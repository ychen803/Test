using log4net;
using SPF.OWAES.Common.Logging;
using SPF.OWAES.DataAccess;
using SPF.OWAES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SPF.OWAES.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WarrantService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WarrantService.svc or WarrantService.svc.cs at the Solution Explorer and start debugging.
    public class WarrantService : IWarrantService
    {
        private static ILog log = Log4Net.GetLog(typeof(WarrantService));
        public List<vwWarrantInfo> GetWarrantList(string id)
        {
            List<vwWarrantInfo> lstWarrant = new List<vwWarrantInfo>();
            try
            {
                IWarrantRepository iRepo = new WarrantRepository();
                lstWarrant = iRepo.GetWarrantList(id);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error in Getting Warrant List :  ", ex.Message.ToString());
            }
            return lstWarrant;
        }

        public DateTime GetLastSuccessfulUpdate()
        {
            DateTime lastUpdated = DateTime.MinValue;
            try
            {
                IWarrantRepository iRepo = new WarrantRepository();
                lastUpdated = iRepo.GetLastSuccessfulUpdate();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error in Getting Last Successful Update:  ", ex.Message.ToString());
            }
            return lastUpdated;
        }


    }
}
