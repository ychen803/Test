using log4net;
using NCS.SecureConnect.SIM.Common.Managers;
using NCS.SecureConnect.SIM.Entities;
using SPF.OWAES.Common.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SPF.OWAES.Controllers
{
    public class ErrorController : Controller
    {
        private static ILog log = Log4Net.GetLog(typeof(WarrantController));
        // GET: Error
        public ActionResult Index()
        {

            Exception err = Session["OWAES_ERROR"] as Exception;
            if (err != null)
            {
                err = err.GetBaseException();
                log.Error("Error Controller: Error has occurred. \n\t" + err.Message, err);

                try { 
                var ssoError = err as  NCS.SecureConnect.SIM.Client.Exceptions.ClientException;
                   
                    switch(ssoError.ErrorCode)
                    {
                        case 109514 : //Login
                            ViewBag.ErrorMessage = err.Message; 
                            break;
                        case 109518:  //Not found - Artifact is missing. Cannot resolve the artifact.
                            return RedirectToAction("NotFound", "Error");
                        case 109515:  //Logout - Logout is unsuccessful  
                        case 109517:   //Logout - Retrieving Logout Request is unsuccessful.
                            ViewBag.ErrorMessage = "Your session has expired. Please login again.";
                            break;
                        default :
                            {break;}

                    }

                    if(err.Message.ToString().Contains("Authentication context not found in session"))
                    { ViewBag.ErrorMessage = "Your session has expired."; }
                   
                }
                catch(Exception ex)
                {
                    ViewBag.ErrorMessage = "An unexpected error occurred on our website. Please email to SPF_WEU@spf.gov.sg or call (+65) 6557 5017 during Warrant Enforcement Unit's operating hours to speak to an officer.";
                    log.Error("Error Controller: Error has occurred. \n\t" + ex.Message, ex);
                }
               
                //if (err.Message.ToString().Contains("Login is unsuccessful."))
                //{
                //    ViewBag.ErrorMessage = err.Message;
                //}
                //else if (err.Message.ToString().Contains("Artifact is missing. Cannot resolve the artifact."))
                //{
                //    return RedirectToAction("NotFound", "Error");
                //}
                //else if (err.Message.ToString().Contains("Retrieving Logout Request is unsuccessful.") || err.Message.ToString().Contains("Authentication context not found in session"))
                //{
                //    ViewBag.ErrorMessage = "Your session has expired.";
                //}
                //else
                //{
                //    ViewBag.ErrorMessage = "An unexpected error occurred on our website. Please email to SPF_WEU@spf.gov.sg or call (+65) 6557 5017 during Warrant Enforcement Unit's operating hours to speak to an officer.";
                //}

                Session["OWAES_ERROR"] = null;
            }
            
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

         //109518 - Artifact is missing. Cannot resolve the artifact
         //109515  - Logout is unsuccessful
        // 109517 - Retrieving Logout Request is unsuccessful.
        //Authentication context not found in session
 
    }
}