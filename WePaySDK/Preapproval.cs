using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net;

namespace WePaySDK
{
    public class Preapproval
    {
        public PreapprovalCreateResponse Post(PreapprovalCreateRequest req)
        {
            PreapprovalCreateResponse response;
            try
            {
                response = new WePayClient().Invoke<PreapprovalCreateRequest, PreapprovalCreateResponse>(req, req.actionUrl, req.accessToken);
            }
            catch(WePayException ex)
            {
                response = new PreapprovalCreateResponse { preapproval_uri = req.redirect_uri+"?error="+ex.error, Error = ex };
            }

            return response;
        }

        public PreapprovalResponse GetStatus(PreapprovalRequest req)
        {
           // var req = new PreapprovalRequest { preapproval_id = preapproval_id };
            PreapprovalResponse response;
            try
            {
                response = new WePayClient().Invoke<PreapprovalRequest, PreapprovalResponse>(req, req.actionUrl);
            }
            catch (WePayException ex)
            {
                response = new PreapprovalResponse { state = ex.error, amount = 0, Error = ex };
            }
            return response;
        }
    }

    public class PreapprovalCreateRequest
    {
        [JsonIgnore]
        public string actionUrl = @"preapproval/create";
        [JsonIgnore]
        public string accessToken { get; set; }

        public long account_id { get; set; }
        public string period { get; set; }
        public string short_description { get; set; }
        public decimal amount { get; set; }
        public string mode { get; set; }
        public string fee_payer { get; set; }
        public decimal app_fee { get; set; } 

        public string reference_id { get; set; }
        public string long_description { get; set; }
        public string callback_uri { get; set; }
        public string redirect_uri { get; set; }
        public string payer_email_message { get; set; }
        public string payee_email_message { get; set; }
    }

    public class PreapprovalCreateResponse
    {
        public long preapproval_id { get; set; }
        public string preapproval_uri { get; set; }

        [JsonIgnore]
        public WePayException Error { get; set; }
      
    }

    public class PreapprovalRequest
    {
        [JsonIgnore]
        public string accessToken { get; set; }
        [JsonIgnore]
        public string actionUrl = @"preapproval";

        public long preapproval_id { get; set; }
    }

    public class PreapprovalResponse
    {
        public long preapproval_id { get; set; }
        public long account_id { get; set; }
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