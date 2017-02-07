using SPF.OWAES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPF.OWAES.DataAccess
{
    public interface ILogRepository
    {
        void WriteLog(LOG log);
    }
}
