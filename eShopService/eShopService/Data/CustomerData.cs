using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace eShopService.Data
{
    [DataContract]
    public class CustomerData
    {
        [DataMember]
        public int customerId { get; set; }

        [DataMember]
        public string customerFName { get; set; }

        [DataMember]
        public string customerLName { get; set; }
        [DataMember]
        public string address { get; set; }

        [DataMember]
        public string phone { get; set; }

        [DataMember]
        public int accountBalance { get; set; }
    }
}
