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

            return View();
        }

        public ActionResult Checkout(int Id)
        {
            var state = new Checkout().GetStatus(Id).state;
            return Content(state);
        }

        public ActionResult CheckoutCreate(decimal amt)
        {
            var hostUrl =this.HttpContext.Request.Url.Scheme+ "://" + this.ControllerContext.HttpContext.Request.Url.Authority;

            var req = new CheckoutCreateRequest
            {
                account_id = WePayConfig.accountId,
                mode = "regular",
                type = "SERVICE",
                amount = amt,
                short_description = "checkout test",
                redirect_uri = hostUrl+ @"/Home/Finish"
            };

            var uri = new Checkout().GetUri(req);
            return Redirect(uri);
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
                redirect_uri = hostUrl + @"/Home/Finish"
            };

            var uri = new Preapproval().GetUri(req);
            return Redirect(uri);
        }

        public ActionResult Finish(string checkout_id, string preapproval_id)
        {
            if (!string.IsNullOrEmpty(checkout_id))
            {
                ViewBag.CartType = "Checkout";
                int id = Convert.ToInt32(checkout_id);
                ViewBag.checkout_id = checkout_id;
                var resp = new WePaySDK.Checkout().GetStatus(id);
                ViewBag.state = resp.state;
                ViewBag.amount = resp.amount;
            }
            else if (!string.IsNullOrEmpty(preapproval_id))
            {
                ViewBag.CartType = "Preapproval";
                int id = Convert.ToInt32(preapproval_id);
                ViewBag.checkout_id = id;
                var resp = new WePaySDK.Preapproval().GetStatus(id);
                ViewBag.state = resp.state;
                ViewBag.amount =  resp.amount;
            }

           
            return View();
        }
      
    }
}
