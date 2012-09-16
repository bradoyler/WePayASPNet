using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net;

namespace WePaySDK
{
    public class Account
    {
        public AccountCreateResponse Post(AccountCreateRequest req)
        {
            AccountCreateResponse response;
            try
            {
                response = new WePayClient().Invoke<AccountCreateRequest, AccountCreateResponse>(req, req.actionUrl, req.accessToken);
            }
            catch(WePayException ex)
            {
                response = new AccountCreateResponse { account_id=0, Error = ex };
            }

            return response;
        }

        public AccountResponse Get(AccountRequest req)
        {
            AccountResponse response;
            try
            {
                response = new WePayClient().Invoke<AccountRequest, AccountResponse>(req, req.actionUrl, req.accessToken);
            }
            catch (WePayException ex)
            {
                response = new AccountResponse { name = ex.error, Error = ex };
            }
            return response;
        }

        public AccountResponse Find(AccountFindRequest req)
        {
            AccountResponse response;
            try
            {
                response = new WePayClient().Invoke<AccountFindRequest, AccountResponse>(req, req.actionUrl, req.accessToken);
            }
            catch (WePayException ex)
            {
                response = new AccountResponse { name = ex.error, Error = ex };
            }
            return response;
        }
    }

    public class AccountCreateRequest
    {
        [JsonIgnore]
        public readonly string actionUrl = @"account/create";
        [JsonIgnore]
        public string accessToken { get; set; }

        public string name { get; set; }
        public string description { get; set; }
        public string reference_id { get; set; }
        public string image_uri { get; set; }
    }

    public class AccountCreateResponse
    {
        public long account_id { get; set; }
        public string account_uri { get; set; }

        [JsonIgnore]
        public WePayException Error { get; set; }
      
    }

    public class AccountRequest
    {
        [JsonIgnore]
        public string accessToken { get; set; }
        [JsonIgnore]
        public readonly string actionUrl = @"account";

        public long account_id { get; set; }
    }

    public class AccountResponse
    {
        public long account_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string reference_id { get; set; }
        public string account_uri { get; set; }
        public decimal payment_limit { get; set; }

        [JsonIgnore]
        public WePayException Error { get; set; }
       
    }

    public class AccountFindRequest
    {
        [JsonIgnore]
        public string accessToken { get; set; }
        [JsonIgnore]
        public readonly string actionUrl = @"account/find";

        public string name { get; set; }
        public string reference_id { get; set; }
    }

}