using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;

namespace WePaySDK
{
    public class WePayClient
    {
        public ResponseT Post<RequestT, ResponseT>(RequestT request, string actionUrl, string accessToken)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Authorization", "Bearer " + accessToken);
            client.Headers.Add("Content-Type", "application/json");
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.60 Safari/537.1");
            var data = JsonConvert.SerializeObject(request);
            string uriString = WePayConfig.endpoint(WePayConfig.ProductionMode) + actionUrl;
            var json = "";

            json = client.UploadString(new Uri(uriString), "POST", data);

            return JsonConvert.DeserializeObject<ResponseT>(json);
        }

        public ResponseT Post<RequestT, ResponseT>(RequestT request, string actionUrl)
        {
            return Post<RequestT, ResponseT>(request, actionUrl, WePayConfig.accessToken);
        }
    }
}