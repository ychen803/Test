using SPF.OWAES.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.Configuration;
using NCS.SecureConnect.SIM.Client;
using NCS.SecureConnect.SIM.Entities;
using NCS.SecureConnect.SIM.Common.Interfaces;

namespace SPF.OWAES.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Home");
            //return View();
        }

       
        public ActionResult Help()
        {
            ViewBag.Message = "Help";

            return View();
        }

        //YNPP
        [Authorize]
        public ActionResult OWAESAcknowledgement()
        {
            ViewBag.Message = "IMPORTANT INFORMATION";

            return View();
        }
 
       
        public ActionResult NoOutstandingWA()
        {
            return View();
        }

        public ActionResult Acknowledge()
        {
            return RedirectToAction("Enquiry", "Warrant");
        }

        public ActionResult Login()
        { 
           return View();
        }

      
        public ActionResult logout()
        {
            return View();
        }

        
    }
}