﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using System.Configuration;

namespace WePaySDK
{
    public static class WePayConfig
    {
        // staging credentials...
        public static string accessToken {get;set;}
        public static int accountId { get; set; }
        public static int clientId { get; set; }
        public static string clientSecret { get; set; }
        public static bool productionMode { get; set; }
        public static string authScope = "manage_accounts,view_balance,collect_payments,refund_payments,view_user,preapprove_payments,send_money";
        public static string endpoint(bool prod)
        {
            if(prod) return @"https://wepayapi.com/v2/";
            return @"https://stage.wepayapi.com/v2/";
        }
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