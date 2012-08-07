using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Text;
using wepayASPNET.WePaySDK;

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
            var state = new Checkout().GetCheckoutStatus(Id).state;
            return Content(state);
        }

        public ActionResult CheckoutCreate(string amt)
        {
            var hostUrl =this.HttpContext.Request.Url.Scheme+ "://" + this.ControllerContext.HttpContext.Request.Url.Authority;
            WePaySDK.WePayConfig.RequestUri = hostUrl;
            var uri = new Checkout().GetCheckoutUri(amt, "test transaction.");
            return Redirect(uri);
        }

        public ActionResult CheckoutFinish(int checkout_id)
        {
            ViewBag.checkout_id = checkout_id;
             var resp = new WePaySDK.Checkout().GetCheckoutStatus(checkout_id);
             ViewBag.state = resp.state;
             ViewBag.amount = resp.amount;
            return View();
        }
      
    }
}
