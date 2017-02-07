using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPF.OWAES.Models;

namespace SPF.OWAES.DataAccess
{
    public class LogRepository : ILogRepository
    {
        WEU_OWAESEntities dbContext;

        public LogRepository()
        {
            dbContext = new WEU_OWAESEntities();
        }
        
        public void WriteLog(LOG log)
        {
            dbContext.LOGs.Add(log);
            dbContext.SaveChanges();
        }
    }
}
