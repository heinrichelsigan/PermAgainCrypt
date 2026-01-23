using EU.CqrXs.Crypt;
using EU.CqrXs.Crypt.EnDeCoding;
using EU.CqrXs.Util;
using EU.CqrXs.Net.NameService;
using System.Net;
using System.Text;

namespace EU.CqrXs.Net.WebHttp
{


    /// <summary>
    /// HttpClientRequest encapsulation
    /// </summary>
    public static class HttpClientRequest
    {

        private static HttpClient httpClientR;
        public static HttpClient HttpClientR { get => httpClientR; }

        private static string topLevelDomain = "at";
        
        static HttpClientRequest()
        {
            // empty static constructor 
        }

        public static HttpClient GetHttpClient(string baseAddr, string hostName, System.Text.Encoding? encoding)
        {
            encoding = encoding ?? Encoding.UTF8;
            baseAddr = (string.IsNullOrEmpty(baseAddr) && !string.IsNullOrEmpty(hostName)) ? $"https://{hostName}/" : baseAddr;
            Uri uri = new Uri(baseAddr);
            
            string hostA = baseAddr.Replace("https://", "").Replace("http://", "");
            hostA = hostA.Contains("/") ? hostA.Substring(0, hostA.IndexOf("/")) : hostA;
            hostA = string.IsNullOrEmpty(hostName) ? hostA : hostName;

            httpClientR = new HttpClient();
            httpClientR.BaseAddress = uri;
            httpClientR.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            httpClientR.DefaultRequestHeaders.Add("AcceptLanguage", "en-US");
            httpClientR.DefaultRequestHeaders.Add("Host", hostA);
            httpClientR.DefaultRequestHeaders.Add("UserAgent", "cqrxs.eu");

            return httpClientR;
        }

        public static HttpClient GetHttpClient(string baseAddr, string secretKey, string hostName = "cqrxs.eu", System.Text.Encoding? encoding = null)
        {
            httpClientR = GetHttpClient(baseAddr, hostName, encoding);
            if (!string.IsNullOrEmpty(secretKey))
            {
                string hexString = Hex16.ToHex16(System.Text.Encoding.UTF8.GetBytes(secretKey));
                httpClientR.DefaultRequestHeaders.Add("Authorization", $"Basic {hexString}");
            }

            return httpClientR;
        }

        public static IDictionary<string, string> GetHeaders(string hostName)
        {
            if (string.IsNullOrEmpty(hostName))
                hostName = "cqrxs.eu";
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
            dict.Add("AcceptLanguage", "en-US");
            dict.Add("Host", hostName);
            dict.Add("UserAgent", "cqrxs.eu");

            return dict;
        }


        /// <summary>
        /// Gets an async <see cref="Task{HttpResponseMessage}"/> for an url
        /// </summary>
        /// <param name="url">url to fetch</param>
        /// <returns><see cref="Task{HttpResponseMessage}"/></returns>
        public async static Task<HttpResponseMessage> GetResponseByUrl(string url)
        {
            Uri uri = new Uri(url);
            string hostName = url.Replace("https://", "").Replace("http://", "");
            if (hostName.Contains('/'))
                hostName = hostName.Substring(0, hostName.IndexOf("/"));

            httpClientR = HttpClientRequest.GetHttpClient(url, hostName, Encoding.UTF8);
            return await httpClientR.GetAsync(uri);
        }

        /// <summary>
        /// Posts a message to the specified <see cref="string">url</see>
        /// </summary>
        /// <param name="url">url to connect</param>
        /// <param name="msg">message to post</param>
        /// <returns>true on http(s) POST success</returns>
        public static bool PostUrlMsg(string url, string msg)
        {
            Uri uri = new Uri(url);
            string hostName = url.Replace("https://", "").Replace("http://", "");
            if (hostName.Contains('/'))
                hostName = hostName.Substring(0, hostName.IndexOf("/"));
            httpClientR = HttpClientRequest.GetHttpClient(url, hostName, Encoding.UTF8);

            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, uri);
            req.RequestUri = uri;
            req.Content = new StringContent(msg);

            HttpResponseMessage res = httpClientR.Send(req);
            return res.IsSuccessStatusCode;
        }

        /// <summary>
        /// Gets the external gateway or host client IP address
        /// </summary>
        /// <param name="urlR">default: https://area23.at/net/R.aspx</param>
        /// <returns>IP Address of client host or in case of SNAT IP Address of 1st internet provider gateway of client host</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IPAddress? GetClientIP(string urlR = "https://area23.at/net/R.aspx")
        {
            string myIp = GetResponseByUrl(urlR).Result.ToString();
            if (myIp.Contains("<body>"))
            {
                myIp = myIp.Substring(myIp.IndexOf("<body>") + 6);
                if (myIp.Contains("</body>"))
                    myIp = myIp.Substring(0, myIp.IndexOf("</body>")).Replace("</body>", "");
            }
            IPAddress ipClient = IPAddress.Parse(myIp);
            List<IPAddress> cqrXsEuIpList = DnsHelper.GetIpAddrsByHostName(Constants.CQRXS_EU);
            cqrXsEuIpList.AddRange(DnsHelper.GetIpAddrsByHostName(Constants.AREA23_AT));
            foreach (IPAddress euIp in cqrXsEuIpList)
            {
                if (euIp == null)
                    continue;
                try
                {
                    if (Extensions.BytesCompare(ipClient.GetAddressBytes(), euIp.GetAddressBytes()) == 0)
                        throw new InvalidOperationException($"{ipClient.AddressFamily} {ipClient.Address} equals {euIp.Address}");
                }
                catch (Exception ex)
                {
                    CException.SetLastException(ex);
                    Area23Log.LogOriginMsgEx("HttpClientRequest", "Error on getting external client ip", ex);
                    return null;
                }
            }

            return ipClient;
        }




    }


}
