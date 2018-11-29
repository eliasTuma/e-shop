using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace eShopService
{
    public partial class eShopServices : eShopService.ISignUpBusinessService
    {
        /// <summary>
        /// Checks if the business id is not used and send back response to the client
        /// </summary>
        /// <param name="businessId">Business Id</param>
        public int CheckBusinessIdAvalibility(int businessId)
        {
            return CheckIfBusinessIdExists(businessId);
        }

        /// <summary>
        /// Creates a new business entity and add it to the database
        /// </summary>
        /// <param name="ownerId">Owner Id</param>
        /// <param name="businessId">Business Id</param>
        /// <param name="businessName">Business Name</param>
        /// <param name="businessAddress">Business Address</param>
        public int SignUpBusiness(int ownerId, int businessId, string businessName, string businessAddress)
        {
            Business newBusiness = new Business
            {
                CustomerId = ownerId,
                BusinessId = businessId,
                BusinessName = businessName,
                Address = businessAddress
            };
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(DateTime.Now + " - " + "Creating new business - "+ businessName);
            }
            dc.Businesses.InsertOnSubmit(newBusiness);
            dc.SubmitChanges();
            return 1;
        }

        /// <summary>
        /// Checks if the business id exists
        /// </summary>
        /// <param name="id">Business Id</param>
        /// <returns>1 if exists, 0 otherwise</returns>
        private int CheckIfBusinessIdExists(int id)
        {
            eShopDBDataContext dc = new eShopDBDataContext();

            if (dc.Businesses.Any(u => u.BusinessId == id))
                return 1;
            return -1;
        }
    }
}
