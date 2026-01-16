using EU.CqrXs.Crypt.Cipher;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Util;
using System.Net;
using System.Text;

namespace EU.Net.WebHttp
{

    /// <summary>
    /// WebClientRequest implements a static WebClient Request via <see cref="WebClient"/>
    /// and mainly provides
    /// <see cref="ExternalClientIpFromServer(string, Encoding?)"/>
    /// <see cref="LatestAtImages(string)"/>
    /// via basic static methods
    /// <see cref="DownloadString(string, string, string, Encoding?)"/>
    /// <see cref="PostMessage(string, string, string, string, Encoding?)"/>
    /// <see cref="DownloadBytes(string, string, Encoding)"/>
    /// funtionality.
    /// </summary>
    public static class WebClientRequest
    {

        #region fields and properties

        private static WebClient wclient;
        public static WebClient WClient { get => wclient; }

        private static readonly WebHeaderCollection headers = new WebHeaderCollection();
        public static WebHeaderCollection Headers { get => headers; }

        private static string topLevelDomain = "at";
        private static string[] UrlImgs
        {
            get => new string[]
                {
                    $"https://search.brave.com/images?q=site%3A{topLevelDomain}&source=web&tf=pd",
                    $"https://duckduckgo.com/?q=site%3A.{topLevelDomain}&df=d&ia=images&iax=images",
                    $"https://www.qwant.com/?q=site%3A{topLevelDomain}&t=images",
                    "https://images.search.yahoo.com/search/images;_ylt=AwriiQVEAWhpR94pfEiJzbkF?p=site%3A" + topLevelDomain + "&fr=yfp-t&imgt=day&fr2=p%3As%2Cv%3Ai"
                };
        }

        #endregion fields and properties

        /// <summary>
        /// static constructor
        /// </summary>
        static WebClientRequest()
        {
            // headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate, br, zstd");

            headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            // TODO:
            // headers.Add(HttpRequestHeader.ContentMd5, "");
            // headers.Add(HttpRequestHeader.From, "true");
            // removed because make trouble in that very .Net1.1 version of WebClient abstraction
            // headers.Add(HttpRequestHeader.KeepAlive, "true");
            // headers.Add(HttpRequestHeader.Connection, "keep-alive");
            headers.Add(HttpRequestHeader.AcceptLanguage, "en-US");
            headers.Add(HttpRequestHeader.Host, "cqrxs.eu");
            headers.Add(HttpRequestHeader.UserAgent, "cqrxs.eu");
            // wclient.BaseAddress = "https://area23.at/";
            // TODO: always forms credentials
            // webclient.Credentials

        }

        #region GetWebClient

        public static WebClient GetWebClient(string baseAddr, string secretKey, string keyIv = "", System.Text.Encoding? encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            wclient = new WebClient();
            wclient.Encoding = encoding;
            if (!string.IsNullOrEmpty(secretKey))
            {
                string hexString = EnDeCodeHelper.KeyToHex(CryptHelper.PrivateUserKey(secretKey));
                if (!string.IsNullOrEmpty(keyIv))
                {
                    hexString = EnDeCodeHelper.KeyToHex(CryptHelper.PrivateKeyWithUserHash(secretKey, keyIv));
                }
                headers.Add(HttpRequestHeader.Authorization, "Basic " + hexString);
            }
            wclient.Headers = headers;
            wclient.BaseAddress = baseAddr;

            return wclient;
        }

        public static WebClient GetWebClient(string baseAddr, System.Text.Encoding? encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            wclient = new WebClient();
            wclient.Encoding = encoding;
            wclient.Headers = headers;
            wclient.BaseAddress = baseAddr;

            return wclient;
        }

        #endregion GetWebClient

        #region basic methods

        /// <summary>
        /// DownloadString downloads a string from an uri
        /// </summary>
        /// <param name="url"></param>
        /// <param name="secretKey"></param>
        /// <param name="keyIv"></param>
        /// <param name="encoding"><see cref="System.Text.Encoding"/></param>
        /// <returns>downloaded string</returns>
        public static string DownloadString(string url, string secretKey, string keyIv = "", System.Text.Encoding? encoding = null)
        {
            WebClient wc = GetWebClient(url, secretKey, keyIv, encoding);
            Uri uri = new Uri(url);
            return wc.DownloadString(uri);
        }

        /// <summary>
        /// DownloadBytes
        /// </summary>
        /// <param name="url"><see cref="T:string">uri</see></param>
        /// <param name="filePath"><see cref="T:string">file path to write downloaded image</see></param>
        /// <param name="encoding"></param>
        /// <returns><see cref="T:FileInfo?" /> of downloaded image file</returns>
        public static FileInfo? DownloadBytes(string url, string filePath, System.Text.Encoding encoding)
        {
            WebClient wc;
            Uri uri;
            encoding = (encoding == null) ? System.Text.Encoding.UTF8 : encoding;

            try
            {
                wc = GetWebClient(url, "", "", encoding);
                uri = new Uri(url);
                wc.DownloadFile(uri.ToString(), filePath);
            }
            catch (Exception exFile)
            {
                Area23Log.LogOriginMsgEx("WebClientRequest", 
                    $"{exFile.GetType()} when downloading from: {url}", exFile);
            }
            if (File.Exists(filePath))
                return new FileInfo(filePath);
                
            return null;
        }

        /// <summary>
        /// PostMessage posts message via <see cref="WebClient.UploadString(string, string)"/>
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="url"></param>
        /// <param name="hostname"></param>
        /// <param name="serverIp"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string PostMessage(string msg, string url, string hostname = "cqrxs.eu", string serverIp = "18.100.254.167", System.Text.Encoding? encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;            
            wclient = new WebClient();
            // TODO: Replace WebClient with one of:
            // WebRequest webRequest = new WebRequest();
            // HttpWebRequest httpWebRequest = new HttpWebRequest();
            wclient.Encoding = encoding;
            headers.Remove(HttpRequestHeader.Host);
            headers.Add(HttpRequestHeader.Host, hostname);
            headers.Remove(HttpRequestHeader.UserAgent);
            headers.Add(HttpRequestHeader.UserAgent, serverIp);
            wclient.Headers = headers;
            wclient.BaseAddress = url;
            string resp = wclient.UploadString(url, msg);

            return resp;
        }

        #endregion basic methods

        /// <summary>
        /// ExternalClientIpFromServer gets external network ip for client from server
        /// </summary>
        /// <param name="url">default: https://cqrxs.eu/net/R.aspx https://area23.at/net/R.aspx</param>
        /// <param name="encoding"><see cref="System.Text.Encoding"/></param>
        /// <returns>external official gateway <see cref="IPAddress">ip address</see> of client</returns>
        public static IPAddress? ExternalClientIpFromServer(string url = "https://cqrxs.eu/net/R.aspx", System.Text.Encoding? encoding = null)
        {
            WebClient wc = GetWebClient(url, encoding);
            Uri uri = new Uri(url);
            string myIp = wc.DownloadString(uri);
            if (myIp.Contains("<body>"))
            {
                myIp = myIp.Substring(myIp.IndexOf("<body>"));
                if (myIp.Contains("</body>"))
                    myIp = myIp.Substring(0, myIp.IndexOf("</body>")).Replace("<body>", "").Replace("</body>", "");
            }
            return IPAddress.Parse(myIp);
        }

        /// <summary>
        /// Gets latest images from .at or other specified top level domain 
        /// searching duckduckgo, brave, qwant and yahoo image search engines
        /// </summary>
        /// <param name="topLvlDomain">top level domain suffix;
        /// e.g. at, de, eu, edu, gov
        /// you could also add a subdomain suffix like: ac.at, co.at, gv.at or similar
        /// </param>
        /// <returns><see cref="List{String}"/> of svg and img html tags</returns>
        public static List<string> LatestAtImages(string topLvlDomain)
        {
            WebClient wc;
            
            List<string> imgList = new List<string>();
            topLevelDomain = string.IsNullOrEmpty(topLvlDomain) ? "at" : topLvlDomain;
            foreach (string url in UrlImgs)
            {
                bool isParsed = false;
                try
                {                    
                    wc = GetWebClient(url, System.Text.Encoding.UTF8);
                    string respToParse = wc.DownloadString(new Uri(url));
                    
                    while (!isParsed)
                    {
                        if (respToParse.Contains("<img", StringComparison.CurrentCultureIgnoreCase))
                        {
                            int imgIdx = respToParse.IndexOf("<img");
                            if (imgIdx < 0)
                                imgIdx = respToParse.IndexOf("<Img");
                            if (imgIdx < 0)
                                imgIdx = respToParse.IndexOf("<IMG");
                            respToParse = respToParse.Substring(imgIdx);

                            string imgToAdd = respToParse.Substring(0, respToParse.IndexOf(">") + 1);
                            imgList.Add(imgToAdd);
                            respToParse = respToParse.Substring(respToParse.IndexOf(">") + 1);                            
                        }
                        else if (respToParse.Contains("<svg"))
                        {
                            respToParse = respToParse.Substring(respToParse.IndexOf("<svg"));
                            string svgToAdd = respToParse.Substring(0, respToParse.IndexOf("</svg>") + 1);
                            imgList.Add(svgToAdd);
                            respToParse = respToParse.Substring(respToParse.IndexOf("</svg>") + 1);                            
                        }
                        else
                        {
                            isParsed = true;
                        }
                    }
                }
                catch (Exception exHttpUrlGet)
                {
                    Area23Log.LogOriginMsgEx("WebClientRequest", "Error on getting latest images from url: " + url, exHttpUrlGet);
                    isParsed = true;
                }

            }

            return imgList.ToArray().Distinct().ToList();
        }

    }

}
