using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
using WePaySDK;

namespace wepayASPNET.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            ViewBag.redirect_uri = this.HttpContext.Request.Url.Scheme + "://" + this.ControllerContext.HttpContext.Request.Url.Authority+ "/Home/OAuth";
            return View();
        }

        public ActionResult Checkout(int Id)
        {
            var state = new Checkout().GetStatus(Id).state;
            return Content(state);
        }

        public ActionResult GetUser(string Id)
        {
            var user = new User().GetUser(Id);
            return Content("Email:" + user.email + " State:" + user.state);
        }

        public ActionResult CheckoutCreate(decimal amt)
        {
            var hostUrl = this.HttpContext.Request.Url.Scheme + "://" + this.ControllerContext.HttpContext.Request.Url.Authority;

            var req = new CheckoutCreateRequest
            {
                account_id = WePayConfig.accountId,
                mode = "regular",
                type = "SERVICE",
                amount = amt,
                short_description = "checkout test",
                redirect_uri = hostUrl + @"/Home/CheckoutStatus"
            };

            var uri = new Checkout().Process(req).checkout_uri;
            return Redirect(uri);
        }

        public ActionResult ProcessPreapproval(int preapproval_id, decimal amt)
        {
            var hostUrl = this.HttpContext.Request.Url.Scheme + "://" + this.ControllerContext.HttpContext.Request.Url.Authority;

            var req = new CheckoutCreateRequest
            {
                account_id = WePayConfig.accountId,
                mode = "regular",
                type = "SERVICE",
                amount = amt,
                short_description = "checkout test",
                redirect_uri = hostUrl + @"/Home/CheckoutStatus",
                preapproval_id = preapproval_id
            };

            var result = new Checkout().Process(req);
            return Redirect(hostUrl + @"/Home/CheckoutStatus?checkout_id=" + result.checkout_id);
        }

        public ActionResult PreapprovalCreate(decimal amt)
        {
            var hostUrl = this.HttpContext.Request.Url.Scheme + "://" + this.ControllerContext.HttpContext.Request.Url.Authority;

            var req = new PreapprovalCreateRequest
            {
                account_id = WePayConfig.accountId,
                mode = "regular",
                amount = amt,
                period = "once",
                short_description = "test pre-approval",
                redirect_uri = hostUrl + @"/Home/PreapprovalStatus"
            };

            var uri = new Preapproval().GetUri(req);
            return Redirect(uri);
        }

        public ActionResult CheckoutStatus(string checkout_id)
        {
            long iid = 0;
            bool valid = Int64.TryParse(checkout_id, out iid);
            if (!valid)
            {
                ViewBag.Msg = "error";
                ViewBag.amount = 0;
                return View("Status");
            }
            ViewBag.checkout_id = iid;
            var resp = new WePaySDK.Checkout().GetStatus(iid);
          
            ViewBag.state = resp.state;
            ViewBag.amount = string.Format("{0:c}", resp.amount);
            ViewBag.Msg = "Checkout " + resp.state;

            return View("Status");
        }

        public ActionResult PreapprovalStatus(string preapproval_id)
        {
            int iid = 0;
            bool valid = Int32.TryParse(preapproval_id, out iid);
            if (!valid)
            {
                ViewBag.Msg = "error";
                ViewBag.amount = 0;
                return View("Status");
            }

            ViewBag.checkout_id = iid;
            var resp = new WePaySDK.Preapproval().GetStatus(iid);
            ViewBag.state = resp.state;
            ViewBag.amount = string.Format("{0:c}", resp.amount);
            ViewBag.Msg = "Preapproval Complete";
            return View("Status");
        }

        public ActionResult OAuth(string code)
        {
            var hostUrl = this.HttpContext.Request.Url.Scheme + "://" + this.ControllerContext.HttpContext.Request.Url.Authority;

            var req = new TokenRequest
            {
                client_id = WePayConfig.clientId,
                client_secret = WePayConfig.clientSecret,
                code = code,
                redirect_uri = hostUrl + @"/Home/OAuth"
            };

            var response = new OAuth().Authorize(req);
            ViewBag.Msg ="UserId:"+ response.user_id+" Token:"+response.access_token;//.Substring(0,7)+"...";
            return View("Status");
        }
    }
}
