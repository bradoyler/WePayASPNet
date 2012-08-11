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
        public CheckoutCreateResponse Process(CheckoutCreateRequest req)
        {
            CheckoutCreateResponse response;
            try
            {
                response = new WePayClient().Post<CheckoutCreateRequest, CheckoutCreateResponse>(req, req.actionUrl);
            }
            catch (WePayException ex) 
            {
                response = new CheckoutCreateResponse { checkout_id = 0, checkout_uri = "/error", Error =ex };
            }

            return response;
        }

        public CheckoutResponse GetStatus(long checkout_id)
        {
            var req = new CheckoutRequest { checkout_id = checkout_id };
            CheckoutResponse response;
            try
            {
                response = new WePayClient().Post<CheckoutRequest, CheckoutResponse>(req, req.actionUrl);
            }
            catch (WePayException ex) 
            {
                response = new CheckoutResponse { state = ex.error, amount = 0, Error = ex };
            }
            return response;
        }
    }

    public class CheckoutCreateRequest
    {
        [JsonIgnore]
        public string actionUrl = @"checkout/create";

        public int account_id { get; set; }
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
        public int preapproval_id { get; set; }
    }

    public class CheckoutCreateResponse
    {
        public int checkout_id { get; set; }
        public string checkout_uri { get; set; }

        [JsonIgnore]
        public WePayException Error { get; set; }
    }

    public class CheckoutRequest
    {
        public long checkout_id { get; set; }

        [JsonIgnore]
        public string actionUrl = @"checkout";
    }

    public class CheckoutResponse
    {
        public int checkout_id { get; set; }
        public int account_id { get; set; }
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

        [JsonIgnore]
        public WePayException Error { get; set; }
    }

}