using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace eShopService.Data
{
    [DataContract]
    public class UserData
    {
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string password { get; set; }
    }
}
