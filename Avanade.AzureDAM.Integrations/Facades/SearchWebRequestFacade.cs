using System.Configuration;
using System.Data.Services.Client;
using System.Diagnostics;
using System.IO;
using System.Net;
using Avanade.AzureDAM.Integrations.Clients;

namespace Avanade.AzureDAM.Integrations.Facades
{
    public class SearchWebRequestFacade
    {
        private readonly WebRequest _request;
        private readonly string _apiKey = ConfigurationManager.AppSettings["SearchServiceKey"];
        private readonly string _body;

        public SearchWebRequestFacade(string managementEndPoint, string method) : this(managementEndPoint, method, string.Empty) { }

        public SearchWebRequestFacade(string managementEndPoint, string method, string body)
        {
            _request = WebRequest.Create(managementEndPoint);
            _request.Headers.Add("api-key", _apiKey);
            _request.ContentType = "application/json";
            _request.Method = method;

            _body = body;
        }
        
        public void Invoke()
        {
            if (_body != string.Empty)
            {
                using (var requestWriter = new StreamWriter(_request.GetRequestStream()))
                {
                    requestWriter.Write(_body);
                    requestWriter.Flush();
                    requestWriter.Close();
                }
            }
            else
                _request.ContentLength = 0;

            try
            {
                _request.GetResponse();
            }
            catch (WebException exception)
            {
                LogHttpError(exception);
            }
        }

        private static void LogHttpError(WebException exception)
        {
            using (var response = (HttpWebResponse)exception.Response)
            {
                Debug.WriteLine("Error code: {0}", response?.StatusCode);
                using (var data = response?.GetResponseStream())
                    if (data != null)
                        using (var reader = new StreamReader(data))
                        {
                            var text = reader.ReadToEnd();
                            Debug.WriteLine(text);
                        }
            }
        }
    }
}