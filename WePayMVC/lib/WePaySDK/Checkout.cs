using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net;

namespace WePaySDK
{
    public class Checkout
    {
        public string GetUri(CheckoutCreateRequest req)
        {
            string resultUri = "";

            using (var client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + WePayConfig.accessToken);
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.60 Safari/537.1");
                var data = JsonConvert.SerializeObject(req);

                string uriString = WePayConfig.endpoint(false) + req.actionUrl;
                var response = client.UploadString(new Uri(uriString), data);
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                resultUri = values["checkout_uri"];
            }
            return resultUri;
        }

        public CheckoutResponse GetStatus(int checkout_id)
        {
            CheckoutResponse chkResponse;
            var req = new CheckoutRequest { checkout_id = checkout_id };

            using (var client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + WePayConfig.accessToken);
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.60 Safari/537.1");
                var data = JsonConvert.SerializeObject(req);

                string uriString = WePayConfig.endpoint(false) + req.actionUrl;
                var response = client.UploadString(new Uri(uriString), data);
                chkResponse = JsonConvert.DeserializeObject<CheckoutResponse>(response);

            }
            return chkResponse;

        }
    }

    public class CheckoutCreateRequest
    {
        [JsonIgnore]
        public string actionUrl = @"checkout/create";

        public string account_id { get; set; }
        public string short_description { get; set; }
        public string type { get; set; }
        public decimal amount { get; set; }
        public string mode { get; set; }

        public string reference_id { get; set; }
        public string long_description { get; set; }
        public string callback_uri { get; set; }
        public string redirect_uri { get; set; }
        public string payer_email_message { get; set; }
        public string payee_email_message { get; set; }
        public string preapproval_id { get; set; }
    }

    public class CheckoutCreateResponse
    {
        public string checkout_id { get; set; }
        public string checkout_uri { get; set; }
    }

    public class CheckoutRequest
    {
        public int checkout_id { get; set; }

        [JsonIgnore]
        public string actionUrl = @"checkout";
    }

    public class CheckoutResponse
    {
        public string checkout_id { get; set; }
        public string account_id { get; set; }
        public string state { get; set; }
        public string short_description { get; set; }
        public string type { get; set; }
        public decimal amount { get; set; }
        public string fee { get; set; }
        public string gross { get; set; }
        public string reference_id { get; set; }
        public string long_description { get; set; }
        public string callback_uri { get; set; }
        public string redirect_uri { get; set; }
        public string payer_email { get; set; }
        public string payer_name { get; set; }
        public string app_fee { get; set; }
        public string currency { get; set; }
        public string fee_payer { get; set; }
        public string cancel_reason { get; set; }
        public string refund_reason { get; set; }
        public string auto_capture { get; set; }
        public string require_shipping { get; set; }
        public string shipping_address { get; set; }
        public string tax { get; set; }
        public decimal amount_refunded { get; set; }
        public string create_time { get; set; }
    }

}