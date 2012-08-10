using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net;

namespace WePaySDK
{
    public class OAuth
    {
        public TokenResponse Authorize(TokenRequest req)
        {
            TokenResponse response;
            try
            {
                response = new WePayClient().Post<TokenRequest, TokenResponse>(req, req.actionUrl );
            }
            catch
            {
                response = new TokenResponse { access_token = "error" };
            }

            return response;
        }
    }

    public class TokenRequest
    {
        public int client_id { get; set; }
        public string redirect_uri { get; set; }
        public string client_secret { get; set; }
        public string code { get; set; }

        [JsonIgnore]
        public string actionUrl = @"oauth2/token";
    }

    public class TokenResponse
    {
        public int user_id { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
    }
}