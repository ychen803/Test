using SPF.OWAES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SPF.OWAES.WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWarrantService" in both code and config file together.
    [ServiceContract]
    public interface IWarrantService
    {
        [OperationContract]
        [WebInvoke( Method="GET", 
                    ResponseFormat=WebMessageFormat.Json, 
                    BodyStyle= WebMessageBodyStyle.Bare,
                    UriTemplate = "Warrant/{id}")]
        List<vwWarrantInfo> GetWarrantList(string id);


        [OperationContract]
        [WebInvoke(Method = "GET",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.Bare,
                    UriTemplate = "LastUpdate")]
        DateTime GetLastSuccessfulUpdate();
    }
}
