using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eShopService.Data;
using System.ServiceModel;
using System.IO;

namespace eShopService
{
    public partial class eShopServices : IMainActivitiesService
    {
        public eShopDBDataContext dc = new eShopDBDataContext();
        public static string path = @"c:\c#-databases\log.txt";


        // My Business Contract's Implementation

        /// <summary>
        /// Checks if the current logged in user have business or not
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool CheckIfCustomerGotBusiness(int customerId)
        {
            if (dc.Businesses.Any(x => x.CustomerId == customerId))
                return true;
            return false;
        }

        /// <summary>
        /// Returns user's id by given username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetOwnerIdFromUsername(string username)
        {
            int id = -1;
            var query = from a in dc.Users where a.Username == username select a.CustomerId;
            foreach(var item in query)
            {
                id = (int)item;
            }

            return id;
        }

        /// <summary>
        /// Removing the given user from online users tabel
        /// </summary>
        /// <param name="username">Username</param>
        public void Logout(string username)
        {
            if(username != "")
            {
                OnlineUser onlineUser = dc.OnlineUsers.SingleOrDefault(x => x.Username == username);
                if(dc.OnlineUsers.Any(x=>x.Username == username))
                {
                    dc.OnlineUsers.DeleteOnSubmit(onlineUser);
                    dc.SubmitChanges();

                    // Save query to log
                    using(StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(DateTime.Now + " - " + username + " logged off");
                    }
                }                
            }                      
        }

        /// <summary>
        /// Returns BusinessData class of the owner
        /// </summary>
        /// <param name="ownerId">Owner id</param>
        /// <returns>Business data class</returns>
        public BusinessData GetBusinessInformation(int ownerId)
        {
            BusinessData business = new BusinessData();
            var query = from a in dc.Businesses where a.CustomerId == ownerId
                        select new { BusinessId = a.BusinessId, BusinessName = a.BusinessName,
                            BusinessAddress = a.Address };

            foreach(var item in query)
            {
                business.address = item.BusinessAddress;
                business.businessId = item.BusinessId;
                business.businessName = item.BusinessName;
            }

            return business;
        }

        /// <summary>
        /// Returns list of products of owner's business
        /// </summary>
        /// <param name="ownerId">Owner id</param>
        /// <returns>List of products</returns>
        public List<ProductData> GetBusinessProducts(int ownerId)
        {
            var query1 = from a in dc.Businesses
                        where a.CustomerId == ownerId
                        select a.BusinessId;
            int businessId = (int)query1.First();

            var query2 = from a in dc.Products
                    where a.BusinessId == businessId
                    select new
                    {
                        ProductName = a.ProductName,
                        ProductId = a.ProductId,
                        ProductPrice = a.Price,
                        ProductKeys = a.Keys
                    };
            List<ProductData> listOfProducts = new List<ProductData>();
            foreach (var item in query2)
            {
                ProductData newProduct = new ProductData();
                newProduct.businessId = businessId;
                newProduct.productId = item.ProductId;
                newProduct.productName = item.ProductName;
                newProduct.price = (float)item.ProductPrice;
                newProduct.keys = item.ProductKeys;

                listOfProducts.Add(newProduct);
            }

            return listOfProducts;
        }

        /// <summary>
        /// Updates business information
        /// </summary>
        /// <param name="businessId">Business id</param>
        /// <param name="businessName">Business name</param>
        /// <param name="businessAddress">Business address</param>
        public void UpdateBusinessInfo(int businessId, string businessName, string businessAddress)
        {
            Business business = dc.Businesses.SingleOrDefault(x => x.BusinessId == businessId);
            business.BusinessName = businessName;
            business.Address = businessAddress;
            dc.SubmitChanges();

            // Save query to log
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(DateTime.Now + " - " +"Updating business info for "+ businessName);
            }

        }

        /// <summary>
        /// Adds new product to the business shop
        /// </summary>
        /// <param name="businessId">Business id</param>
        /// <param name="productId">Product id</param>
        /// <param name="productName">Product name</param>
        /// <param name="productPrice">Product price</param>
        /// <param name="productKeys">Product keys</param>
        /// <returns>1 - success, -1 product id already exists</returns>
        public int AddProductForBusiness(int businessId, int productId, string productName, float productPrice, string productKeys)
        {
            if (dc.Products.Any(x => x.ProductId == productId))
            {
                return -1;
            }
            else
            {
                Product newProdcut = new Product()
                {
                    BusinessId = businessId,
                    ProductId = productId,
                    ProductName = productName,
                    Price = productPrice,
                    Keys = productKeys
                };
                dc.Products.InsertOnSubmit(newProdcut);
                dc.SubmitChanges();

                // Save query to log
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(DateTime.Now + " - " +"Adding "+ productName + " to " + "Business id number "+ businessId);
                }
            }
            return 1;
        }

        /// <summary>
        /// Deletes certain product by business owner
        /// </summary>
        /// <param name="productId">Product id</param>
        public void DeleteProductForBusiness(int productId)
        {
            Product product = dc.Products.SingleOrDefault(x => x.ProductId == productId);
            dc.Products.DeleteOnSubmit(product);
            dc.SubmitChanges();

            // Save query to log
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(DateTime.Now + " - " +"Deleting product number " +productId);
            }
        }

        /// <summary>
        /// Updates the product information given by the business owner
        /// </summary>
        /// <param name="businessId">Business id</param>
        /// <param name="productId">Product id</param>
        /// <param name="productName">Product name</param>
        /// <param name="productPrice">Product price</param>
        /// <param name="productKeys">Product Keys</param>
        public void UpdateProductInfo(int businessId, int productId, string productName, float productPrice, string productKeys)
        {
            Product product = dc.Products.SingleOrDefault(x => x.ProductId == productId);
            product.ProductName = productName;
            product.Price = productPrice;
            product.Keys = productKeys;

            // Save query to log
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(DateTime.Now + " - " + "Updating product information for " + product.ProductName);
            }

            dc.SubmitChanges();

           
        }


        // Search Contract's Implementation

        /// <summary>
        /// Sends response message to client including the list of the products from search result
        /// </summary>
        /// <param name="searchText">Product name or sub-name</param>
        /// <param name="inRange">True - include price range. False otherwise</param>
        /// <param name="from">Price from</param>
        /// <param name="to">Price to</param>
        /// <returns>List of matches products</returns>
        public List<ProductData> SearchByProduct(string searchText, bool inRange, int from, int to)
        {
            List<ProductData> listOfProduct = new List<ProductData>();

            var productsListResult = (inRange) ?
            from q in dc.Products
            where (q.Keys.Contains(searchText) && (q.Price > @from && q.Price < @to))
            select new
            {
                ProductBusinessId = q.BusinessId,
                ProductId = q.ProductId,
                ProductName = q.ProductName,
                ProductPrice = q.Price
            } :
            from r in dc.Products
            where (r.Keys.Contains(searchText))
            select new
            {
                ProductBusinessId = r.BusinessId,
                ProductId = r.ProductId,
                ProductName = r.ProductName,
                ProductPrice = r.Price
            };


            foreach (var item in productsListResult)
            {
                ProductData product = new ProductData();
                product.businessId = item.ProductBusinessId;
                product.productId = item.ProductId;
                product.price = (float)item.ProductPrice;
                product.productName = item.ProductName;
                product.businessName = (from a in dc.Businesses where a.BusinessId == (int)item.ProductBusinessId select a.BusinessName).SingleOrDefault(); 
                listOfProduct.Add(product);
            }
            return listOfProduct;
        }

        /// <summary>
        /// Sends response message to client including the list of the products from search result
        /// </summary>
        /// <param name="searchText">Business name or sub-name</param>
        /// <param name="inRange">True - include price range. False otherwise</param>
        /// <param name="from">Price from</param>
        /// <param name="to">Price to</param>
        /// <returns>List of matches products</returns>
        public List<ProductData> SearchByBusiness(string searchText, bool inRange, int from, int to)
        {
            List<ProductData> listOfProduct = new List<ProductData>();

            var productsListResult = (inRange) ?
            from q in dc.Products
            where (q.Business.BusinessName.Contains(searchText) && (q.Price > @from && q.Price < @to))
            select new
            {
                ProductBusinessId = q.BusinessId,
                ProductId = q.ProductId,
                ProductName = q.ProductName,
                ProductPrice = q.Price
            } :
            from r in dc.Products
            where (r.Business.BusinessName.Contains(searchText))
            select new
            {
                ProductBusinessId = r.BusinessId,
                ProductId = r.ProductId,
                ProductName = r.ProductName,
                ProductPrice = r.Price
            };


            foreach (var item in productsListResult)
            {
                ProductData product = new ProductData();
                product.businessId = item.ProductBusinessId;
                product.productId = item.ProductId;
                product.price = (float)item.ProductPrice;
                product.productName = item.ProductName;
                product.businessName = (from a in dc.Businesses where a.BusinessId == (int)item.ProductBusinessId select a.BusinessName).SingleOrDefault();
                listOfProduct.Add(product);
            }
            return listOfProduct;
        }

        /// <summary>
        /// Checks if the buyer got money to buy the cart
        /// </summary>
        /// <param name="cartCost">Cart cost</param>
        /// <param name="buyerId">Buyer id</param>
        /// <returns>True if he got the money, false otherwise</returns>
        public bool CheckBalanceBeforePurchase(int cartCost, int buyerId)
        {
            int buyerBalance =(int) (from a in dc.Customers
                                where a.CustomerId == buyerId
                                select a.AccountBalance).FirstOrDefault();

            return (cartCost > buyerBalance) ? false : true;
        }

        /// <summary>
        /// Adds new transaction between buyer and seller and updates their account balance.
        /// </summary>
        /// <param name="cartList">Items in cart</param>
        /// <param name="buyerId">Buyer id</param>
        /// <param name="cartCost">Total cart cost</param>
        /// <param name="itemsTotalPrice">Cost of each item in cart</param>
        public void BuyCart(List<ProductData> cartList, int buyerId,List<float> itemsTotalPrice)
        {
            int index = 0, sellerId = 0;
            foreach (var item in cartList)
            {
                sellerId = (from a in dc.Businesses where a.BusinessId == item.businessId select a.CustomerId).FirstOrDefault();
                Transaction newTransaction = new Transaction()
                {
                    Buyer = (from a in dc.Customers where a.CustomerId == buyerId select a.FirstName).FirstOrDefault(),
                    Seller = (from a in dc.Businesses where a.BusinessId == item.businessId select a.Customer.FirstName).FirstOrDefault(),
                    ProductName = item.productName,
                    Price = itemsTotalPrice[index]
                };
                // Save query to log
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(DateTime.Now + " - " + "Adding new transaction between "+ newTransaction.Buyer +" and " +newTransaction.Seller + ". After that updating their account balance");
                }

                dc.Transactions.InsertOnSubmit(newTransaction);
                dc.SubmitChanges();

                // Update buyer and seller account balance

                Customer s = dc.Customers.SingleOrDefault(x => x.CustomerId == sellerId);
                s.AccountBalance += (int)itemsTotalPrice[index];
                dc.SubmitChanges();

                Customer b = dc.Customers.SingleOrDefault(x => x.CustomerId == buyerId);
                b.AccountBalance -= (int)itemsTotalPrice[index];
                dc.SubmitChanges();

                index++;
            }
        }
        private int GetSellerAndBuyerIdByBusinessId(int businessId)
        {
            int ownerId = (from a in dc.Businesses where a.BusinessId == businessId select a.CustomerId).FirstOrDefault();
            return ownerId;
        }
        /// <summary>
        /// Checks if no items have been removed before the purchase
        /// </summary>
        /// <param name="cartList">List of items in cart</param>
        /// <returns>True if all items exist, False otherwise</returns>
        public int ValidateCartItems(List<ProductData> cartList)
        {
            int indexToDelete = 0;
            foreach(var item in cartList)
            {
                if (!dc.Products.Any(x => x.ProductId == item.productId))
                    return indexToDelete;
                indexToDelete++;
            }
            return -1;
        }

        /// <summary>
        /// Creates new list of products that contains only the existing ones in the database
        /// </summary>
        /// <param name="oldCartList">Old cart items</param>
        /// <returns>Updated cart items</returns>
        public List<ProductData> GetUpdatedCartList(List<ProductData> oldCartList)
        {
            List<ProductData> updatedCartList = new List<ProductData>();

            foreach(var item in oldCartList)
            {
                if(dc.Products.Any(x=>x.ProductId == item.productId))
                {
                    ProductData p = new ProductData();
                    p.businessId = item.businessId;
                    p.businessName = item.businessName;
                    p.productId = item.productId;
                    p.productName = item.productName;
                    p.price = item.price;
                    p.keys = item.keys;

                    updatedCartList.Add(p);
                }
            }
            return updatedCartList;
        }

        /// <summary>
        /// Gets transaction data for the current logged in user
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        public List<TransactionData> GetTransactionData(int ownerId)
        {
            string ownerName = (from a in dc.Customers where a.CustomerId == ownerId select a.FirstName).FirstOrDefault();
            List<TransactionData> transactionList = new List<TransactionData>();

            var query = from a in dc.Transactions
                        where a.Buyer == ownerName || a.Seller == ownerName
                        select new
                        {
                            Id = a.Id,
                            Buyer = a.Buyer,
                            Seller = a.Seller,
                            ProductName = a.ProductName,
                            Price = a.Price
                        };
            foreach(var item in query)
            {
                TransactionData data = new TransactionData(item.Id, item.Buyer, item.Seller,item.ProductName, (float)item.Price);
                transactionList.Add(data);
            }

            return transactionList;
        }

        /// <summary>
        /// Gets customer information by his id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public CustomerData GetCustomerInformation(int customerId)
        {
            CustomerData customer = new CustomerData();
            var c = (from a in dc.Customers
                     where a.CustomerId == customerId
                     select new
                     {
                         CustomerId = a.CustomerId,
                         FirstName = a.FirstName,
                         LastName = a.LastName,
                         Address = a.Address,
                         Phone = a.Phone,
                         AccountBalance = a.AccountBalance
                     }).FirstOrDefault();

            customer.customerId = c.CustomerId;
            customer.customerFName = c.FirstName;
            customer.customerLName = c.LastName;
            customer.address = c.Address;
            customer.phone = c.Phone;
            customer.accountBalance = (int)c.AccountBalance;

            return customer;
            
        }
        /// <summary>
        /// Updates customer information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fName"></param>
        /// <param name="lName"></param>
        /// <param name="address"></param>
        /// <param name="phone"></param>
        /// <param name="accountBalance"></param>
        public void UpdateCustomerInfo(int id, string fName, string lName, string address, string phone, float accountBalance)
        {
            Customer c = dc.Customers.FirstOrDefault(x => x.CustomerId == id);
            c.FirstName = fName;
            c.LastName = lName;
            c.Address = address;
            c.Phone = phone;
            c.AccountBalance = (int)accountBalance;

            // Save query to log
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(DateTime.Now + " - " + "Updating customer personal information for " +c.FirstName + " " + c.LastName);
            }

            dc.SubmitChanges();
        }
    }
}
