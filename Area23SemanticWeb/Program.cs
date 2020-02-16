using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Area23SemanticWeb
{
  public class Program
  {
    static readonly HttpClient client = new HttpClient();

    static void Main(string[] args) =>
      AsyncMain(args).ConfigureAwait(true);

    static async Task AsyncMain(string[] args)
    {
      JsonConfig.ConfigObject jsonConfig = new JsonConfig.ConfigObject();
      var jsonTrends = ReadJsonConfig(ref jsonConfig);
      string fetchOutput = await FetchAllTrends(jsonTrends);

      Console.WriteLine("Hello World!" + fetchOutput);
    }
      
    public static List<string> ReadJsonConfig(ref JsonConfig.ConfigObject jsonConfig)
    {
      List<string> fetchUrlTrends = new List<string>();

      System.IO.FileInfo jsonFi = new System.IO.FileInfo("C:\\Users\\HeinrichElsigan\\source\\repos\\Area23SemanticWeb\\Area23SemanticWeb\\trendfetch.json");
      jsonConfig = JsonConfig.Config.ApplyJsonFromFileInfo(jsonFi);

      foreach (var key in jsonConfig.Keys)
      {
        var jsonvalue = jsonConfig[key];
        // fetchUrlTrends.Add(jsonvalue);
        foreach (var trendUrl in (string[])jsonvalue)
        {
          fetchUrlTrends.Add(trendUrl);
        }
      }

      return fetchUrlTrends;
    }
 

    public static async Task<string> FetchAllTrends(List<string> fetchUris)
    {
      string fetchOutput = string.Empty;

      foreach (var jsonUri in fetchUris)
      {
        fetchOutput += await FetchDataFromUri(jsonUri);
      }

      return fetchOutput;
    }


    public static async Task<string> FetchDataFromUri(string myUri)
    {
      string fetchedFromUri = string.Empty;       
      try
      {
        WebRequest request = WebRequest.Create(myUri);
        request.Credentials = CredentialCache.DefaultCredentials;
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        // WebResponse wResponse = await request.GetResponseAsync();
        Stream dataStream = response.GetResponseStream();
        StreamReader reader = new StreamReader(dataStream);
        fetchedFromUri = reader.ReadToEnd();
        reader.Close();
        dataStream.Close();
        // wResponse.Close();
        response.Close();
      }
      catch (Exception e)
      {
        fetchedFromUri = "\nException Caught! Message :" + e.Message;
        Console.Error.WriteLine(fetchedFromUri);
        throw;
      }
      return fetchedFromUri;
    }


    public static async Task<string> FetchUriData(string myUri)
    {
      string fetchedFromUri = string.Empty;
      try
      {
        HttpResponseMessage response = await client.GetAsync(myUri);
        response.EnsureSuccessStatusCode();
        fetchedFromUri = await response.Content.ReadAsStringAsync();
        // Above three lines can be replaced with new helper method below
        // string responseBody = await client.GetStringAsync(uri);

        Console.WriteLine(fetchedFromUri);
      }
      catch (Exception e)
      {
        fetchedFromUri = "\nException Caught! Message :" + e.Message;
        Console.WriteLine(fetchedFromUri);
      }

      return fetchedFromUri;
    }

  }




}
