using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace WePaySDK
{
    public class WePayClient
    {
        public ResponseT Invoke<RequestT, ResponseT>(RequestT request, string actionUrl, string accessToken)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Authorization", "Bearer " + accessToken);
            client.Headers.Add("Content-Type", "application/json");
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.60 Safari/537.1");
            var data = JsonConvert.SerializeObject(request);
            string uriString = WePayConfig.endpoint(WePayConfig.ProductionMode) + actionUrl;
            var json = "";
            try
            {
                if (data.Length > 3)
                {
                    json = client.UploadString(new Uri(uriString), "POST", data);
                }
                else { json = client.DownloadString(new Uri(uriString)); }
            }
            catch (WebException we)
            {
                if (we.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse httpErrorResponse = (HttpWebResponse)we.Response as HttpWebResponse;

                    StreamReader reader = new StreamReader(httpErrorResponse.GetResponseStream(), Encoding.UTF8);
                    string responseBody = reader.ReadToEnd();
                    var errResp = JsonConvert.DeserializeObject<ErrorResponse>(responseBody);
                    throw new WePayException { error = errResp.error, error_description=errResp.error_description, error_message =we.Message};
                }
                else
                {
                    throw we;
                }
            }
            return JsonConvert.DeserializeObject<ResponseT>(json);
        }

        public ResponseT Invoke<RequestT, ResponseT>(RequestT request, string actionUrl)
        {
            return Invoke<RequestT, ResponseT>(request, actionUrl, WePayConfig.accessToken);
        }
    }

    public class ErrorResponse
    {
        public string error { get; set; }
        public string error_description { get; set; }
    }

    public class WePayException : Exception
    {
        public string error { get; set; }
        public string error_description { get; set; }
        public string error_message { get; set; }
    }
}