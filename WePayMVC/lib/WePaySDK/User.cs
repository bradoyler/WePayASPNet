using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net;

namespace WePaySDK
{
    public class User
    {
        public UserRegisterResponse Register(UserRegisterRequest req)
        {
            UserRegisterResponse response;
            try
            {
                response = new WePayClient().Post<UserRegisterRequest, UserRegisterResponse>(req, req.actionUrl);
            }
            catch
            {
                response = new UserRegisterResponse { access_token = "error" };
            }

            return response;
        }

        public UserResponse GetUser(string accessToken)
        {
            UserRequest req = new UserRequest { accessToken = accessToken };
            UserResponse response;
            try
            {
                response = new WePayClient().Post<UserRequest, UserResponse>(req, req.actionUrl, accessToken);
            }
            catch
            {
                response = new UserResponse { state = "error" };
            }

            return response;
        }
    }

    public class UserRegisterRequest
    {
        [JsonIgnore]
        public string actionUrl = @"user/register";

        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string email { get; set; }
        public string scope { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string original_ip { get; set; }
        public string original_device { get; set; }
        public string redirect_uri { get; set; }
    }

    public class UserRegisterResponse
    {
        public string user_id { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
    }

    public class UserRequest
    {
        [JsonIgnore]
        public string accessToken { get; set; }

        [JsonIgnore]
        public string actionUrl = @"user";
    }

    public class UserResponse
    {
        public string user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string state { get; set; }
    }

}