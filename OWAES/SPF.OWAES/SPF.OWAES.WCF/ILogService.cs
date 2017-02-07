using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SPF.OWAES.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ILogService" in both code and config file together.
    [ServiceContract]
    public interface ILogService
    {
        //[OperationContract]
        //[WebInvoke(Method = "PUT",
        //          ResponseFormat = WebMessageFormat.Xml,
        //          BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //          UriTemplate = "Log/{id}")]
        //void WriteLog(string id);

        
        [OperationContract]
        [WebInvoke(Method = "GET",
                  ResponseFormat = WebMessageFormat.Json,
                  BodyStyle = WebMessageBodyStyle.Bare,
                  UriTemplate = "Log/{id}")]
        string WriteLog(string id);
    }
}
