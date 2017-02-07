using SPF.OWAES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPF.OWAES.DataAccess
{
    public class WarrantRepository : IWarrantRepository
    {
        WEU_OWAESEntities dbContext;

        public WarrantRepository()
        {
            dbContext = new WEU_OWAESEntities();
        }

        public List<vwWarrantInfo> GetWarrantList(string id)
        {
           return dbContext.vwWarrantInfoes.Where(w => w.PersonIDNo == id).ToList();
        }
        
        public DateTime GetLastSuccessfulUpdate()
        {
            return dbContext.vwLastSuccessfulUpdates.FirstOrDefault().LastSuccessfulUpdate;
        }
      
    }
}
