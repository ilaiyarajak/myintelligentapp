using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyLove.Services
{
    public class Luis
    {
        private static string LUISUrl = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/d4a0b92e-7197-4168-b565-e8cfb2a46e83?subscription-key=af91fcfbb1b8437cb7d7c41b712b32b6&verbose=true&timezoneOffset=0&q=";
        
        public Luis() { }
        public Task<string> MakeAsyncRequest(string url,string querystring,string type)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + querystring);
            request.ContentType = type;
            request.Method = "GET";

            Task<WebResponse> task = Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                (object)null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result));
        }
        public Task<string> MakeAsyncRequest(string querystring)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(LUISUrl+querystring);
            request.ContentType = "application/json";
            request.Method = "GET";

            Task<WebResponse> task = Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                (object)null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result));
        }

        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(responseStream))
            {
                //Need to return this response 
                string strContent = sr.ReadToEnd();
                return strContent;
            }
        }
    }
    
    #region My Classes
    public class BotProcessing
    {
    }
    #endregion
    #region LUIS response
    public class LuisResponce
    {
        public string query { get; set; }
        public TopScoringIntent topScoringIntent { get; set; }
        public List<Intents> intents { get; set; }
        public List<Entities> entities { get; set; }

        public LuisResponce() { }
    }
    public class TopScoringIntent
    {
        public string intent { get; set; }
        public string score { get; set; }
        public TopScoringIntent() { }
    }
    public class Intents
    {
        public string intent { get; set; }
        public string score { get; set; }
        public Intents() { }
    }
    public class Entities
    {
        public string entity { get; set; }
        public string type { get; set; }
        public int startIndex { get; set; }
        public int endIndex { get; set; }
        public string score { get; set; }
        public Entities() { }
    }
    #endregion
}
