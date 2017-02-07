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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LogService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select LogService.svc or LogService.svc.cs at the Solution Explorer and start debugging.
    public class LogService : ILogService
    {
        public string WriteLog(string id)
        {
            ILogRepository iRepo = new LogRepository();
            LOG userLog = new LOG();
            userLog.LogID = Guid.NewGuid();
            userLog.PersonIDNo = id;
            userLog.LogDateTime = DateTime.Now;
            iRepo.WriteLog(userLog);
            return "success";
        }
    }
}
