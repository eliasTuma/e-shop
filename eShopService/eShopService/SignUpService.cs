using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace eShopService
{
    public partial class eShopServices : ISignUpService
    {
        /// <summary>
        /// Checks if the customer id exists and sends a reponse to the client
        /// </summary>
        /// <param name="id">Customer id</param>
        public void CheckIdAvailability(int id)
        {
            int response = CheckIfIdExists(id);
            
            OperationContext.Current.GetCallbackChannel<ISignUpServiceCallBack>().IdAvailabilityResponse(response);
        }

        /// <summary>
        /// Checks if the username already exists and sends a reponse to the client
        /// </summary>
        /// <param name="username"></param>
        public void CheckUsernameAvailability(string username)
        {
            int response = CheckIfUsernameExists(username);

            OperationContext.Current.GetCallbackChannel<ISignUpServiceCallBack>().UsernameAvailabilityResponse(response);
        }

        /// <summary>
        /// Creates User and Customer entities and adds them to the database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="customerId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="address"></param>
        /// <param name="phone"></param>
        /// <param name="accountBalance"></param>
        public void SignUp(string username, string password, int customerId, string firstName,
            string lastName, string address, string phone, int accountBalance)
        {
            eShopDBDataContext dc = new eShopDBDataContext();
            
            if (CheckIfUsernameExists(username) == 1)
            {
                OperationContext.Current.GetCallbackChannel<ISignUpServiceCallBack>().ResponseToSignUp(-1);
            }
            else 
            {

                if(CheckIfIdExists(customerId) == 1)
                    OperationContext.Current.GetCallbackChannel<ISignUpServiceCallBack>().ResponseToSignUp(-2);
                else
                {
                    User newUser = new User()
                    {
                        Username = username,
                        Password = password,
                        CustomerId = customerId
                    };
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(DateTime.Now + " - " + "Creating new user and new customer for "+firstName+" "+lastName);
                    }
                    dc.Users.InsertOnSubmit(newUser);
                    dc.SubmitChanges();

                    Customer newCustomer = new Customer()
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        Phone = phone,
                        CustomerId = customerId,
                        AccountBalance = accountBalance
                    };
                    dc.Customers.InsertOnSubmit(newCustomer);
                    dc.SubmitChanges();

                    OperationContext.Current.GetCallbackChannel<ISignUpServiceCallBack>().ResponseToSignUp(1);
                }
            }
        }

        private int CheckIfUsernameExists(string username)
        {
            eShopDBDataContext dc = new eShopDBDataContext();
            if (dc.Users.Any(u => u.Username == username))
                return 1;
            return -1;
        }

        private int CheckIfIdExists(int id)
        {
            eShopDBDataContext dc = new eShopDBDataContext();

            if (dc.Customers.Any(u => u.CustomerId == id))
                return 1;
            return -1;
        }
    }
}
