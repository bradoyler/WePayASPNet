using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
using System.Configuration;
using WePaySDK;

namespace wepayASPNET.Controllers
{
    public class HomeController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            base.OnActionExecuting(ctx);
            GlobalVars.hostUrl =Request.Url.Scheme + "://" + Request.Url.Authority;
        }
       
        public ActionResult Index()
        {
            ViewBag.redirect_uri = GlobalVars.hostUrl + "/Home/OAuth";
            return View();
        }

        public ActionResult Checkout(int Id)
        {
            var resp = new Checkout().GetStatus(Id);
            if (resp.Error != null)
            {
                var msg = resp.Error.error + " - " + resp.Error.error_description;
                return Content(msg);
            }

            return Content(resp.state);
        }

        public ActionResult GetUser(string Id)
        {
            var resp = new User().GetUser(Id);
            if (resp.Error != null)
            {
                ViewBag.Error = resp.Error.error + " - " + resp.Error.error_description;
                return View("Status");
            }
            ViewBag.state = resp.state;
            ViewBag.Msg = "User " + resp.state+" - "+resp.email;
            return View("Status");
        }

        public ActionResult CheckoutCreate(decimal amt)
        {
          
            var req = new CheckoutCreateRequest
            {
                account_id = WePayConfig.accountId,
                mode = "regular", accessToken=WePayConfig.accessToken,
                type = "SERVICE",
                amount = amt,
                short_description = "checkout test",
                redirect_uri = GlobalVars.hostUrl + @"/Home/CheckoutStatus"
            };

            var resp = new Checkout().Post(req);
            if (resp.Error != null)
            {
                ViewBag.Error = resp.Error.error + " - " + resp.Error.error_description;
                return View("Status");
            }

            return Redirect(resp.checkout_uri);
        }

        public ActionResult ProcessPreapproval(int preapproval_id, decimal amt)
        {
            var req = new CheckoutCreateRequest
            {
                account_id = WePayConfig.accountId, accessToken=WePayConfig.accessToken,
                mode = "regular",
                type = "SERVICE",
                amount = amt,
                short_description = "checkout test",
                redirect_uri = GlobalVars.hostUrl + @"/Home/CheckoutStatus",
                preapproval_id = preapproval_id
            };

            var resp = new Checkout().Post(req);
            if (resp.Error != null)
            {
                ViewBag.Error = resp.Error.error + " - " + resp.Error.error_description;
                return View("Status");
            }
            return Redirect(GlobalVars.hostUrl + @"/Home/CheckoutStatus?checkout_id=" + resp.checkout_id);
        }

        public ActionResult PreapprovalCreate(decimal amt)
        {
            var req = new PreapprovalCreateRequest
            {
                account_id = WePayConfig.accountId, accessToken=WePayConfig.accessToken,
                mode = "regular",
                amount = amt,
                period = "once",
                short_description = "test pre-approval",
                redirect_uri = GlobalVars.hostUrl + @"/Home/PreapprovalStatus"
            };

            var resp = new Preapproval().Post(req);
            if (resp.Error != null)
            {
                ViewBag.Error = resp.Error.error + " - " + resp.Error.error_description;
                return View("Status");
            }
            return Redirect(resp.preapproval_uri);
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
            if (resp.Error != null)
            {
                ViewBag.Error = resp.Error.error + " - " + resp.Error.error_description;
                return View("Status");
            }

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
            var req = new PreapprovalRequest { accessToken = WePayConfig.accessToken, preapproval_id = iid };
            var resp = new WePaySDK.Preapproval().GetStatus(req);
            if (resp.Error != null)
            {
                ViewBag.Error = resp.Error.error + " - " + resp.Error.error_description;
                return View("Status");
            }

            ViewBag.state = resp.state;
            ViewBag.amount = string.Format("{0:c}", resp.amount);
            ViewBag.Msg = "Preapproval Complete";
            return View("Status");
        }

        public ActionResult OAuth(string code)
        {
            var req = new TokenRequest
            {
                client_id = WePayConfig.clientId,
                client_secret = WePayConfig.clientSecret,
                code = code,
                redirect_uri = GlobalVars.hostUrl + @"/Home/OAuth"
            };

            var resp = new OAuth().Authorize(req);
            if (resp.Error != null)
            {
                ViewBag.Error = resp.Error.error + " - " + resp.Error.error_description;
                return View("Status");
            }

            var randomNum= new Random(1).Next(99999);
            var accRequest = new AccountCreateRequest { accessToken = resp.access_token, name = "testSDK " + randomNum, description = "test account for SDK demo", reference_id = "test" + randomNum };
            var accResponse = new Account().Post(accRequest);

            ViewBag.Msg = "UserId:" + resp.user_id + " Token:" + resp.access_token;//.Substring(0,7)+"...";
            ViewBag.Msg +="New Account#:"+ accResponse.account_id;
            return View("Status");
        }
    }
}