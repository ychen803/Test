using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;
using System.Configuration;
using SPF.OWAES.Models;
using System.Net;
using System.IO;
using NCS.SecureConnect.SIM.Entities;
using NCS.SecureConnect.SIM.Common.Managers;
using NCS.SecureConnect.SIM.Logging.Utilities;
using log4net;
using SPF.OWAES.Common.Logging;

namespace SPF.OWAES.Controllers
{
  //testcomment  [Authorize]
    public class WarrantController : Controller
    {
        private static ILog log = Log4Net.GetLog(typeof(WarrantController));

        // GET: Warrant
        public ActionResult Enquiry()
        {
            try
            { //YNPP
                AuthenticationToken userToken = SessionManager.GetAuthenticationToken();
                ViewBag.Title = "Enquiry";
                log.Debug("Enquiry Page Load - Getting User Token ...");
                ViewBag.LoginInfo = userToken.Username; // "T5001018F"; 
                log.DebugFormat("Enquiry Page Load - User Token : {0}", userToken.Username);
                WriteLoginLog(ViewBag.LoginInfo);
                log.Debug("Enquiry Page Load - Preparing to display result ...");
                Display();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Enquiry Page Load - {0} ", ex.Message.ToString());
            }
            return View();
        }

        public void Display()
        {
            log.Debug("Enquiry Display - Getting all warrant to display ...");
            List<vwWarrantInfo> lstWarrant = GetWarrant();
            string result = "";
            log.Debug("Enquiry Display - Checking if no warrant ...");
            if (lstWarrant != null)
            {
                log.Debug("Enquiry Display - Warrant result is not null.");
                try
                {
                    string Agency = "";
                    string startWrapper = "<div id='st-accordion' class='st-accordion'> " + "<ul style='padding-left:30px'>";
                    string endWrapper = "</ul> </div>";

                    string ActiveWAList = "";
                    ViewBag.WarrantCount = lstWarrant.Count();
                    log.DebugFormat("Enquiry Display - Warrant count : {0}", lstWarrant.Count());
                    if (lstWarrant.Count > 0)
                    {
                        ViewBag.Instruction = lstWarrant[0].MsgContent;
                        Agency = "<p style='color:#B22222;padding-left:2%;padding-top:30px;font-weight:bold;'>You have <u>" + lstWarrant.Count + "</u> outstanding Warrant of Arrest</p>";
                        Agency += "<p style='padding-left:3%;padding-top:10px;'><strong>Agency(s)</strong></p>";
                        List<string> strCaseType = (from w in lstWarrant select w.CaseTypeDescription).Distinct().ToList();

                        foreach (string cType in strCaseType)
                        {
                            List<vwWarrantInfo> lstFiltered = lstWarrant.Where(w => w.CaseTypeDescription == cType).ToList();
                            if (lstFiltered.Count > 0)
                                ActiveWAList += "<li class='licasetype'><a href='#' class='CaseType'>" + cType + "&nbsp; : &nbsp;" + lstFiltered.Count() + " " + " <span class='st-arrow'>Open or Close</span></a>" +
                                               "   <div class='st-content'>";

                            ActiveWAList += "   <div class='row' style='border-bottom:1px solid #eaeaea;border-top:1px solid #eaeaea; padding:0px;'>";
                            ActiveWAList += "     <div class='col-sm-1 col-xs-12'></div>";
                            ActiveWAList += "     <div class='col-sm-5 col-xs-12'><strong>Warrant No.</strong></div>";
                            ActiveWAList += "      <div class='col-sm-6 col-xs-12'><strong>Summons No. / Report No. </strong></div>";
                            ActiveWAList += "  </div>";
                            ActiveWAList += "  <div class='row'>";

                            int wacount = 1;
                            foreach (vwWarrantInfo wa in lstFiltered)
                            {
                                ActiveWAList += " <div class='col-sm-1 col-xs-12'>" + wacount + ". </div>";
                                ActiveWAList += " <div class='col-sm-5 col-xs-12'>" + wa.WarrantNo.TrimStart().TrimEnd() + "</div>";
                                ActiveWAList += " <div class='col-sm-6 col-xs-12'>" + wa.SummonsNo.TrimStart().TrimEnd() + "</div>";
                                wacount++;

                            }
                            ActiveWAList += "  </div>";
                            ActiveWAList += "   </div> </li>";
                        }
                    }
                    else
                    {
                        ActiveWAList = "<div style='text-align:justify'>" +
                                        "<br/> For further verification, please send an email to SPF_WEU@spf.gov.sg or call 6557 5017 and speak to our operator." +
                                        "</div>";
                    }
                    result += Agency + startWrapper + ActiveWAList + endWrapper;

                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Enquiry Display - Error while formatting outstanding warrants form for ID : {0} ", ViewBag.LoginInfo);
                    log.Error("Enquiry Display - Error has occurred. \n\t" + ex.Message, ex);
                    ViewBag.WarrantCount = -1;
                }
            }
            else
            {
                result = "<div style='margin: 0 auto; text-align: center; width:70%; line-height:2.5em; padding-top:20px; font-size:18px; '>An unexpected error occurred on our website. Please send an email to SPF_WEU@spf.gov.sg or call 6557 5017 and speak to our operator.</div>";
                ViewBag.WarrantCount = -1;
            }

            ViewBag.ActiveWarrantList = result;

            log.Debug("Enquiry Display - Getting Last Successful Update...");
            SetLastSuccessfulUpdate(); 

        }

        public List<vwWarrantInfo> GetWarrant()
        {
            try
            {
                //YNPP
                AuthenticationToken userToken = SessionManager.GetAuthenticationToken();
                List<vwWarrantInfo> listWA = new List<vwWarrantInfo>();
                string BaseServiceURL = ConfigurationManager.AppSettings["BaseServicesURL"];
                string PersonIDNo = userToken.Username; //"T5001018F"; //userToken.Username;  
                string WarrantServiceURL = ConfigurationManager.AppSettings["WarrantServiceURL"];
                string requestURL = BaseServiceURL + WarrantServiceURL + PersonIDNo;
                log.DebugFormat("Enquiry Get Warrant - Warrant URL : {0}", requestURL);
                var request = WebRequest.Create(requestURL);
                request.Method = "GET";
                request.ContentType = "application/json";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream respStream = response.GetResponseStream())
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<vwWarrantInfo>));
                        listWA = serializer.ReadObject(response.GetResponseStream()) as List<vwWarrantInfo>;
                    }
                }
                else
                {
                    //Write Log (Failed)
                    log.ErrorFormat("Error while calling service : {0}  for ID : {1} ", requestURL, ViewBag.LoginInfo);
                    log.ErrorFormat("Status Code: {0}, Status Description: {1}", response.StatusCode, response.StatusDescription);
                    Console.WriteLine(string.Format("Status Code: {0}, Status Description: {1}", response.StatusCode, response.StatusDescription));
                    return null;

                }
                return listWA;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error while calling service : {0}  for ID : {1} ", ex.Message, ex.StackTrace);
                return null;
            }



        }

        public void SetLastSuccessfulUpdate()
        {
            string BaseServiceURL = ConfigurationManager.AppSettings["BaseServicesURL"];
            string WarrantServiceURL = ConfigurationManager.AppSettings["UpdateStatusURL"];
            string requestURL = BaseServiceURL + WarrantServiceURL;

            log.DebugFormat("Enquiry Display - Last Successful Update URL : {0}", requestURL);
            var request = WebRequest.Create(requestURL);
            request.Method = "GET";
            request.ContentType = "application/json";


            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (Stream respStream = response.GetResponseStream())
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DateTime));
                    DateTime LastUpdatedTime = Convert.ToDateTime(serializer.ReadObject(response.GetResponseStream()));

                    ViewBag.LastSuccessfulUpdateTime = LastUpdatedTime.ToString("dddd, dd MMM yyyy hh:mm tt");
                }
            }
            else
            {
                //Write Log (Failed)
                log.ErrorFormat("Enquiry Last Update - Error while calling service : {0}  for ID : {1} ", requestURL, ViewBag.LoginInfo);
                log.ErrorFormat("Enquiry Last Update - Status Code: {0}, Status Description: {1}", response.StatusCode, response.StatusDescription);
            }

        }

        public void WriteLoginLog(string LoginPersonID)
        {
            try
            {
                string BaseServiceURL = ConfigurationManager.AppSettings["BaseServicesURL"];
                string LogServiceURL = ConfigurationManager.AppSettings["LogServiceURL"];
                string requestURL = BaseServiceURL + LogServiceURL + LoginPersonID;

                log.DebugFormat("Enquiry Log - Log URL : {0}", requestURL);

                var request = WebRequest.Create(requestURL);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.ContentLength = 0;

                log.InfoFormat("Write logging log : Before");

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                log.InfoFormat("After");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    log.InfoFormat("Write logging log : 'SUCCESS' for ID : {0} ", ViewBag.LoginInfo);
                }
                else
                {
                    log.ErrorFormat("Error while calling service : {0}  for ID : {1} ", requestURL, ViewBag.LoginInfo);
                    log.ErrorFormat("Status Code: {0}, Status Description: {1}", response.StatusCode, response.StatusDescription);
                }
                log.InfoFormat("Write logging log : End");
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error in WriteLogIn : {0}{1} ", ex.Message.ToString(), ex.StackTrace);
            }
        }


    }
}