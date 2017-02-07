using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SPF.OWAES.Models 
{
    [DataContract]
    public class Warrant
    {
        [DataMember]
        public string WarrantNo { get; set; }

        [DataMember]
        public string SummonsNo { get; set; }

        [DataMember]
        public string CaseType { get; set; }
    }
}