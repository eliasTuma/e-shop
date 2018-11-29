using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace eShopService.Data
{
    [DataContract]
    public class BusinessData
    {
        [DataMember]
        public int ownerId { get; set; }

        [DataMember]
        public int businessId { get; set; }

        [DataMember]
        public string businessName { get; set; }

        [DataMember]
        public string address { get; set; }
    }
}
