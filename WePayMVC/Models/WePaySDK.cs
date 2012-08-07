using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;

namespace wepayASPNET.WePaySDK
{
    public static class WePayConfig
    {
        public static string RequestUri { get; set; }

        // staging credentials...
        public static string accessToken = "a47e76ee76fb4b1df6883804c6290a8d303ae19bd14902040f4b82e1f3ea849b";
        public static string accountId = "478442";
        public static string endpoint(bool prod)
        {
            if(prod) return @"https://www.wepayapi.com/v2/";
            return @"https://stage.wepayapi.com/v2/";
        }
    }

    public class Checkout
    {
        public string finishUrl = WePayConfig.RequestUri + @"/Home/CheckoutFinish";
        public string GetCheckoutUri(string amount, string desc)
        {
            string checkoutUri = "";

            var req = new CheckoutCreateRequest
            {
                account_id = WePayConfig.accountId,
                mode = "regular",
                type = "SERVICE",
                amount = amount,
                short_description = desc,
                redirect_uri = finishUrl
            };

            using (var client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + WePayConfig.accessToken);
                client.Headers.Add("Content-Type", "application/json");
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.1 (KHTML, like Gecko) Chrome/21.0.1180.60 Safari/537.1");
                var data = JsonConvert.SerializeObject(req);

                string uriString = WePayConfig.endpoint(false) + req.actionUrl;
                var response = client.UploadString(new Uri(uriString), data);
                Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(response);
                checkoutUri = values["checkout_uri"];
            }
            return checkoutUri;
        }

        public CheckoutResponse GetCheckoutStatus(int checkout_id)
        {
            CheckoutResponse chkResponse;
            var req = new CheckoutRequest
            {  checkout_id=checkout_id };

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
        public string amount { get; set; }
        public string mode { get; set; }

        public string reference_id { get; set; }
        public string long_description { get; set; }
        public string callback_uri { get; set; }
        public string redirect_uri { get; set; }
        public string payer_email_message { get; set; }
        public string payee_email_message { get; set; } //preapproval_id
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
        public string amount { get; set; }
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
        public string amount_refunded { get; set; }
        public string create_time { get; set; }
    }

    public static class ExtensionMethods
    {
        public static double JsonDate(this DateTime dt)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = dt.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return Math.Round(ts.TotalMilliseconds, 0);
        }
    }
}