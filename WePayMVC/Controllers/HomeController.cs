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
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Checkout(int Id)
        {
            var state = new Checkout().GetCheckoutStatus(Id).state;
            return Content(state);
        }

        public ActionResult CheckoutCreate()
        {
            var uri = new Checkout().GetCheckoutUri("12.50", "test transaction.");
            return Redirect(uri);
        }

      
    }
}
