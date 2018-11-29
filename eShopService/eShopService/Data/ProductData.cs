using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace eShopService.Data
{
    [DataContract]
    public class ProductData
    {
        [DataMember]
        public int productId { get; set; }

        [DataMember]
        public int businessId { get; set; }

        [DataMember]
        public string businessName { get; set; }

        [DataMember]
        public string productName { get; set; }

        [DataMember]
        public float price { get; set; }

        [DataMember]
        public string keys { get; set; }
    }
}
