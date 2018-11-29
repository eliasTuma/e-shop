using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace eShopService.Data
{
    [DataContract]
    public class TransactionData
    {
        public TransactionData(int id,string bName, string sName,string productName,float _price)
        {
            this.transactionId = id;
            this.buyerName = bName;
            this.sellerName = sName;
            this.productName = productName;
            this.price = _price;
        }

        [DataMember]
        int transactionId { set; get; }

        [DataMember]
        string buyerName { set; get; }

        [DataMember]
        string sellerName { set; get; }

        [DataMember]
        string productName { set; get; }

        [DataMember]
        float price { set; get; }
    }
}
