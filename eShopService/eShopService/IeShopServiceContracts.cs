using eShopService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace eShopService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    #region Services
    [ServiceContract (CallbackContract = typeof(ILoginServiceCallBack))]
    public interface ILoginService
    {
        [OperationContract]
        void Login(string username, string password);
  
    }
    
    [ServiceContract (CallbackContract =typeof(ISignUpServiceCallBack))]
    public interface ISignUpService
    {
        [OperationContract]
        void SignUp(string username, string password, int customerId, string firstName,
            string lastName, string address, string phone, int accountBalance);

        [OperationContract]
        void CheckUsernameAvailability(string username);

        [OperationContract]
        void CheckIdAvailability(int id);
    }

    [ServiceContract]
    public interface ISignUpBusinessService
    {
        [OperationContract]
        int SignUpBusiness(int ownerId, int businessId, string businessName, string businessAddress);

        [OperationContract]
        int CheckBusinessIdAvalibility(int businessId);
    }

    [ServiceContract ]
    public interface IMainActivitiesService
    {

        // Profile Tab Contracts
        [OperationContract]
        void UpdateCustomerInfo(int id, string fName, string lName, string address, string phone, float accountBalance);

        // My Business Contracts
        [OperationContract]
        bool CheckIfCustomerGotBusiness(int customerId);

        [OperationContract]
        int GetOwnerIdFromUsername(string username);     

        [OperationContract]
        void Logout(string username);

        [OperationContract]
        Data.BusinessData GetBusinessInformation(int ownerId);

        [OperationContract]
        void UpdateBusinessInfo(int businessId, string businessName, string businessAddress);

        [OperationContract]
        List<Data.ProductData> GetBusinessProducts(int ownerId);

        [OperationContract]
        int AddProductForBusiness(int businessId, int productId, string productName, float productPrice, string productKeys);

        [OperationContract]
        void DeleteProductForBusiness(int productId);

        [OperationContract]
        void UpdateProductInfo(int businessId, int productId, string productName, float productPrice, string productKeys);

        // Search Products Contracts

        [OperationContract]
        List<ProductData> SearchByProduct(string searchText, bool inRange, int from, int to);

        [OperationContract]
        List<ProductData> SearchByBusiness(string searchText, bool inRange, int from, int to);

        [OperationContract]
        bool CheckBalanceBeforePurchase(int cartCost, int buyerId);

        [OperationContract]
        void BuyCart(List<ProductData> cartList, int buyerId, List<float> itemsTotalPrice);

        [OperationContract]
        int ValidateCartItems(List<ProductData> cartList);

        [OperationContract]
        List<ProductData> GetUpdatedCartList(List<ProductData> oldCartList);

        [OperationContract]
        List<TransactionData> GetTransactionData(int ownerId);

        [OperationContract]
        CustomerData GetCustomerInformation(int customerId);

    }
    #endregion

    #region ServicesCallBacks
    public interface ISignUpServiceCallBack
    {
        [OperationContract]
        void ResponseToSignUp(int response);

        [OperationContract]
        void UsernameAvailabilityResponse(int response);

        [OperationContract]
        void IdAvailabilityResponse(int response);
    }

    public interface ILoginServiceCallBack
    {
        [OperationContract]
        void ResponseToLogin(int response);

        [OperationContract]
        void ResponseToLogout();
    }

    #endregion


}
