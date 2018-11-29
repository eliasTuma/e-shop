using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eShopClient
{
    public partial class MainForm : Form
    {
        public static string usernameLogedIn;
        public static int ownerId;
        public static eShopServiceReference.MainActivitiesServiceClient mainActivity = new eShopServiceReference.MainActivitiesServiceClient();
        private delegate void invokeRequieredDelegate();
        private delegate void updateProductsGridItems(int pId, string pName, float pPrice, string pKeys);
        private delegate void updateBusinessNameDelegate(string _name, string _address);
        private delegate void updateProductInfoDelegate(int selectedIndex, int businessId, int productId, string productName, float price, string productKeys);
        private eShopServiceReference.BusinessData businessInfo;
        private List<int> quantitiesList = new List<int>();
        private List<eShopServiceReference.ProductData> cartList = new List<eShopServiceReference.ProductData>();
        private List<float> itemsTotalCostList = new List<float>();
        private eShopServiceReference.ProductData[] searchResult = null;



        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Welcome To eSHOP, please login in order to user the application. \n For More Information About the app click Help->About", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
            tabBarsControllers.Visible = false;
            tabBarsControllers.SelectedIndex = 2;
            logoutToolStripMenuItem.Enabled = false;

            // Set profile details for the first time
            customerIdTxt.Enabled = false;
            fNameTxt.Enabled = false;
            lNameTxt.Enabled = false;
            addressTxt.Enabled = false;
            phoneTxt.Enabled = false;
            accountBalanceTxt.Enabled = false;
            saveBtn.Visible = false;
            searchByProductRadio.Checked = true;

            
            for (int i = 1; i <= 100; i++)
            {
                quantityComboBox.Items.Add(i);
            }
            quantityComboBox.SelectedIndex = 0;

        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login newLoginForm = new Login();
            newLoginForm.updateGUI += new Login.updateGUIDelegate(updateGUI);
            newLoginForm.ShowDialog();
        }

        /// <summary>
        /// Executed by Login Form to update the eShop interface after the login
        /// </summary>
        public void updateGUI()
        {

            if (this.InvokeRequired)
            {
                invokeRequieredDelegate d = new invokeRequieredDelegate(updateGUI);
                this.BeginInvoke(d);
            }
            else {
                loginToolStripMenuItem.Enabled = false;
                tabBarsControllers.Visible = true;
                logoutToolStripMenuItem.Enabled = true;

                ownerId = mainActivity.GetOwnerIdFromUsername(usernameLogedIn);                
            }

        }

        /// <summary>
        /// Request list of products for the current business and add them to gridview
        /// </summary>
        private void GetBusinessProducts()
        {
            var listOfProducts = mainActivity.GetBusinessProducts(ownerId);
            foreach (var item in listOfProducts)
            {
                businessProductsGrid.Rows.Add(item.productId, item.productName, item.price, item.keys);
            }
        }

        /// <summary>
        /// On click on logout strip menu, executes logout service and updates the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cartDataGrid.Rows.Clear();
            itemsTotalCostList.Clear();
            quantitiesList.Clear();
            cartList.Clear();
            businessProductsGrid.Rows.Clear();
            tabBarsControllers.SelectedIndex = 2;
            searchResultGrid.Rows.Clear();
            mainActivity.Logout(usernameLogedIn);
            usernameLogedIn = "";
            tabBarsControllers.Visible = false;
            loginToolStripMenuItem.Enabled = true;
            logoutToolStripMenuItem.Enabled = false;

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            // In case the form closed and the user didnt logout, execute logout for the current user
            if (usernameLogedIn != "")
            {
                mainActivity.Logout(usernameLogedIn);
                usernameLogedIn = "";
                mainActivity.Close();
            }
        }


        #region MyBusiness TabBar Methods
        /// <summary>
        /// Creates and open new BusinessInfo form to update the the customer's business info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            BusinessInfo business = new BusinessInfo(businessInfo.businessName, businessInfo.address);
            business.updateDelegate += new BusinessInfo.updateBusinessInformation(UpdateBusinessInfo);
            business.ShowDialog();
        }

        /// <summary>
        /// Executed by EditBusiness form to update business info
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_address"></param>
        private void UpdateBusinessInfo(string _name, string _address)
        {

            if (this.businessNameLbl.InvokeRequired)
            {
                updateBusinessNameDelegate u = new updateBusinessNameDelegate(UpdateBusinessInfo);
                this.BeginInvoke(u, new object[] { _name, _address });
            }
            else
            {
                this.businessNameLbl.Text = _name;
                mainActivity.UpdateBusinessInfo(businessInfo.businessId, _name, _address);
            }
        }

        private void addProductBtn_Click(object sender, EventArgs e)
        {
            AddNewProduct add = new AddNewProduct(businessInfo.businessId);
            add.updateProductsList += new AddNewProduct.updateProductGrid(updateProductGridView);
            add.ShowDialog();
        }

        /// <summary>
        /// Executed by AddProduct form using delegate.
        /// Adds new us row that contains the new product to the gridview
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productName"></param>
        /// <param name="price"></param>
        /// <param name="productKeys"></param>
        private void updateProductGridView(int productId, string productName, float price, string productKeys)
        {
            if (this.InvokeRequired)
            {
                updateProductsGridItems u = new updateProductsGridItems(updateProductGridView);
                this.BeginInvoke(u, new object[] { productId, productName, price, productKeys });

            }
            else
            {
                businessProductsGrid.Rows.Add(productId, productName, price, productKeys);
            }

        }

        /// <summary>
        /// Create new EditProduct form and send the the product, business and selected index parameters
        /// in order to edit them in the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editProductBtn_Click(object sender, EventArgs e)
        {
            int selectedIndex, pId;
            float price;
            string pName, pKeys;
            if (businessProductsGrid.SelectedRows.Count > 1)
            {
                MessageBox.Show("Please select 1 product only to edit it information.");
                return;
            }
            else
            {
                selectedIndex = businessProductsGrid.SelectedRows[0].Index;
                pId = Int32.Parse(businessProductsGrid.SelectedRows[0].Cells[0].Value.ToString());
                pName = businessProductsGrid.SelectedRows[0].Cells[1].Value.ToString();
                price = float.Parse(businessProductsGrid.SelectedRows[0].Cells[2].Value.ToString());
                pKeys = businessProductsGrid.SelectedRows[0].Cells[3].Value.ToString();
                EditProduct product = new EditProduct(selectedIndex, businessInfo.businessId, pId, pName, price, pKeys);
                product.updateProductInfo += new EditProduct.updateProductInfoDelegate(this.updateProductInfo);
                product.Show();

            }
        }

        /// <summary>
        ///  Executed by EditProduct Form using delegate to update the product we wanted to edit its info
        /// </summary>
        /// <param name="selectedIndex"></param>
        /// <param name="businessId"></param>
        /// <param name="productId"></param>
        /// <param name="productName"></param>
        /// <param name="price"></param>
        /// <param name="productKeys"></param>
        private void updateProductInfo(int selectedIndex, int businessId, int productId, string productName, float price, string productKeys)
        {
            if (this.InvokeRequired)
            {
                updateProductInfoDelegate u = new updateProductInfoDelegate(updateProductInfo);
                this.BeginInvoke(u, new object[] { selectedIndex, businessId, productId, productName, price, productKeys });
            }
            else
            {
                businessProductsGrid.Rows[selectedIndex].Cells[0].Value = productId;
                businessProductsGrid.Rows[selectedIndex].Cells[1].Value = productName;
                businessProductsGrid.Rows[selectedIndex].Cells[2].Value = price;
                businessProductsGrid.Rows[selectedIndex].Cells[3].Value = productKeys;
            }
        }

        private void deleteProductBtn_Click(object sender, EventArgs e)
        {
            int productId;
            if (businessProductsGrid.SelectedRows.Count > 1)
            {
                MessageBox.Show("Please select 1 product only.");
                return;
            }
            else
            {
                productId = (int)businessProductsGrid.SelectedRows[0].Cells[0].Value;
                mainActivity.DeleteProductForBusiness(productId);
                businessProductsGrid.Rows.RemoveAt(businessProductsGrid.SelectedRows[0].Index);
                MessageBox.Show("The product has been removed successfuly.");
            }
        }
        #endregion

        #region SearchProduct TabBar Methods
        private void searchTxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            searchResultGrid.Rows.Clear();
            if (searchTxt.Text == "")
                return;

            // Search by product
            if (searchByProductRadio.Checked)
            {
                if (priceRangeCheckBox.Checked)
                {
                    if (!CheckIfValidPriceRange())
                    {
                        return;
                    }
                    searchResult = mainActivity.SearchByProduct(searchTxt.Text, true, int.Parse(rangeFromTxt.Text), int.Parse(rangeToTxt.Text));
                }
                else
                {
                    searchResult = mainActivity.SearchByProduct(searchTxt.Text, false, 0, 0);
                }
            }

            // Search by business
            if (searchByBusinessRadio.Checked)
            {
                if (priceRangeCheckBox.Checked)
                {
                    if (!CheckIfValidPriceRange())
                    {
                        return;
                    }
                    searchResult = mainActivity.SearchByBusiness(searchTxt.Text, true, int.Parse(rangeFromTxt.Text), int.Parse(rangeToTxt.Text));
                }
                else
                {
                    searchResult = mainActivity.SearchByBusiness(searchTxt.Text, false, 0, 0);
                }
            }

            foreach (var item in searchResult)
            {
                searchResultGrid.Rows.Add(item.productName, item.price, item.businessName);
            }
        }

        private void searchTxt_Click(object sender, EventArgs e)
        {
            searchTxt.Text = "";
        }

        bool CheckIfValidPriceRange()
        {
            if (rangeFromTxt.Text == "" || rangeToTxt.Text == "")
            {
                MessageBox.Show("Range Field must contain numbers.", "Information messing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            try
            {
                float from = float.Parse(rangeFromTxt.Text);
                float to = float.Parse(rangeToTxt.Text);
                if (from < 0 || to < 0)
                {
                    MessageBox.Show("Range Field must be > 0.", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (from > to)
                {
                    MessageBox.Show("Price range from must be > than to.", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Range Field must contain numbers only.", "Invalid Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void searchProductsTab_Selected(object sender, TabControlEventArgs e)
        {
            searchByProductRadio.Checked = true;
        }

        private void addToCartBtn_Click(object sender, EventArgs e)
        {
            if (searchResultGrid.SelectedRows.Count < 1)
            {
                MessageBox.Show("You need to select atleast one item.", "Error while adding to cart", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int rowsCount = searchResultGrid.SelectedRows.Count;

            for (int i = 0; i < rowsCount; i++)
            {
                int productId = GetProductIdByProdcutName(searchResultGrid.SelectedRows[i].Cells[0].Value.ToString());
                eShopServiceReference.ProductData tmp = GetProductFromSearchResultList(productId);
                cartList.Add(tmp);

                int quantity = int.Parse(quantityComboBox.SelectedItem.ToString());
                float totalCostForItem = quantity * tmp.price;
                itemsTotalCostList.Add(totalCostForItem);
                quantitiesList.Add(quantity);

                cartDataGrid.Rows.Add(tmp.productName, quantityComboBox.SelectedItem, totalCostForItem + " = " + tmp.price + " X " + quantity);
                totalPriceLbl.Text = (float.Parse(totalPriceLbl.Text) + totalCostForItem).ToString();
            }

        }


        private eShopServiceReference.ProductData GetProductFromSearchResultList(int productId)
        {
            eShopServiceReference.ProductData product = new eShopServiceReference.ProductData();

            for (int i = 0; i < searchResult.Length; i++)
            {
                if (searchResult[i].productId == productId)
                {
                    product.productId = searchResult[i].productId;
                    product.productName = searchResult[i].productName;
                    product.price = searchResult[i].price;
                    product.keys = searchResult[i].keys;
                    product.businessId = searchResult[i].businessId;
                    product.businessName = searchResult[i].businessName;
                    return product;
                }
            }
            return null;
        }

        private float GetPriceByProductName(string productName)
        {
            for (int i = 0; i < searchResult.Length; i++)
            {
                if (searchResult[i].productName == productName)
                    return searchResult[i].price;
            }
            return -1;
        }
        private int GetProductIdByProdcutName(string productName)
        {
            for (int i = 0; i < searchResult.Length; i++)
            {
                if (searchResult[i].productName == productName)
                    return searchResult[i].productId;
            }
            return -1;
        }

        private void removeFromCartBtn_Click(object sender, EventArgs e)
        {
            if (cartDataGrid.SelectedRows.Count < 1)
            {
                MessageBox.Show("You need to select atleast one item to remove.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int selectedGridRowIndex = cartDataGrid.CurrentRow.Index;

            for (int i = 0; i < cartDataGrid.SelectedRows.Count; i++)
            {
                // update total price label
                float totalPrice = float.Parse(totalPriceLbl.Text),
                    itemPrice = GetPriceByProductName(cartDataGrid.SelectedRows[i].Cells[0].Value.ToString());

                totalPrice -= float.Parse(cartDataGrid.SelectedRows[i].Cells[1].Value.ToString()) * itemPrice;
                totalPriceLbl.Text = totalPrice.ToString();

                // remove item from cart list
                int indexToRemove = 0;

                for (int j = 0; j < cartList.Count; j++)
                {
                    if (cartDataGrid.SelectedRows[i].Cells[0].Value.ToString() == cartList[j].productName)
                        indexToRemove = j;
                }

                cartList.RemoveAt(indexToRemove);

                // remove item from cart grid
                cartDataGrid.Rows.RemoveAt(cartDataGrid.SelectedRows[i].Index);

                // remove total price from itemTotatlPriceList
                itemsTotalCostList.RemoveAt(selectedGridRowIndex);

                // remove quantity from quantityList
                quantitiesList.RemoveAt(selectedGridRowIndex);
                selectedGridRowIndex = cartDataGrid.CurrentRow.Index;
            }
        }

        private void buyBtn_Click(object sender, EventArgs e)
        {
            if(cartDataGrid.SelectedRows.Count < 1)
            {
                MessageBox.Show("You need to add items to your cart first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int result = mainActivity.ValidateCartItems(cartList.ToArray()), index = 0;
            float newTotalSum = 0;

            // Gets the new updated cartList with all the exisiting products
            cartList = (mainActivity.GetUpdatedCartList(cartList.ToArray())).ToList();

            // Keep removing products from cart that has been removed by business owner
            while (result != -1)
            {
                quantitiesList.RemoveAt(result);
                itemsTotalCostList.RemoveAt(result);
                result = mainActivity.ValidateCartItems(cartList.ToArray());
            }

            // Clear the cart grid and add the updated list
            cartDataGrid.Rows.Clear();
            foreach (var item in cartList)
            {
                cartDataGrid.Rows.Add(item.productName, quantitiesList[index], itemsTotalCostList[index] + " = " + item.price + " X " + quantitiesList[index]);
                newTotalSum += itemsTotalCostList[index];
                index++;
            }

            totalPriceLbl.Text = newTotalSum.ToString();

            DialogResult res = MessageBox.Show("Would you like to add new items before completing the purchase ?", "Buy Cart", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(res == DialogResult.Yes)
            {
                return;
            }
            else
            {
                mainActivity.BuyCart(cartList.ToArray(), ownerId, itemsTotalCostList.ToArray());
                MessageBox.Show("Congratulations, your purchase has been successfuly completed. \n You can check all the transactions you have made in Transactions tab.", "Purchase Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            cartDataGrid.Rows.Clear();
        }

        #endregion

        private void transactionsTab_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Modify the tab bar body and requests depending on which tab bar has been choosen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchProductsTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabBarsControllers.SelectedIndex == 3)
            {
                transactionDataGrid.Rows.Clear();
                List<eShopServiceReference.TransactionData> transactionData = new List<eShopServiceReference.TransactionData>();

                transactionData = (mainActivity.GetTransactionData(ownerId)).ToList();
                if (transactionData.Count > 0)
                {
                    foreach (var item in transactionData)
                    {
                        transactionDataGrid.Rows.Add(item.transactionId, item.buyerName, item.sellerName, item.productName, item.price);
                    }
                }
            }
            if(tabBarsControllers.SelectedIndex == 0)
            {
                eShopServiceReference.CustomerData c = mainActivity.GetCustomerInformation(ownerId);
                customerIdTxt.Text = c.customerId.ToString();
                fNameTxt.Text = c.customerFName;
                lNameTxt.Text = c.customerLName;
                addressTxt.Text = c.address;
                phoneTxt.Text = c.phone;
                accountBalanceTxt.Text = c.accountBalance.ToString();
            }
            if(tabBarsControllers.SelectedIndex == 1)
            {
                bool result = mainActivity.CheckIfCustomerGotBusiness(ownerId);
                if (result == false)
                {
                    businessGroupBox.Visible = false;
                    DialogResult res = MessageBox.Show("You don't have a shop yet, would you like to open one ?", "New business", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(res == DialogResult.Yes)
                    {
                        NewBusiness newBusiness = new NewBusiness(ownerId.ToString());
                        newBusiness.ShowDialog();
                        businessInfo = mainActivity.GetBusinessInformation(ownerId);
                        businessNameLbl.Text = businessInfo.businessName;
                        businessGroupBox.Visible = true;
                        GetBusinessProducts();
                    }
                }
                businessInfo = mainActivity.GetBusinessInformation(ownerId);
            }
        }

        /// <summary>
        /// On click get the updated transaction list from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshBtn_Click(object sender, EventArgs e)
        {
            transactionDataGrid.Rows.Clear();
            List<eShopServiceReference.TransactionData> transactionData = new List<eShopServiceReference.TransactionData>();

            transactionData = (mainActivity.GetTransactionData(ownerId)).ToList();
            if (transactionData.Count > 0)
            {
                foreach (var item in transactionData)
                {
                    transactionDataGrid.Rows.Add(item.transactionId, item.buyerName, item.sellerName, item.productName, item.price);
                }

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (editInformationCheckBox.Checked == false)
            {
                customerIdTxt.Enabled = false;
                fNameTxt.Enabled = false;
                lNameTxt.Enabled = false;
                addressTxt.Enabled = false;
                phoneTxt.Enabled = false;
                accountBalanceTxt.Enabled = false;
                saveBtn.Visible = false;
            }
            else
            {
                fNameTxt.Enabled = true;
                lNameTxt.Enabled = true;
                addressTxt.Enabled = true;
                phoneTxt.Enabled = true;
                accountBalanceTxt.Enabled = true;
                saveBtn.Visible = true;
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            float balance;
            
            try
            {
                balance = float.Parse(accountBalanceTxt.Text);
                if(balance < 0)
                {
                    MessageBox.Show("Account balance must be greater than 0.", "Invalid account balance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Account balance must contain only numbers.", "Invalid account balance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            mainActivity.UpdateCustomerInfo(int.Parse(customerIdTxt.Text), fNameTxt.Text, lNameTxt.Text,addressTxt.Text, phoneTxt.Text,float.Parse(accountBalanceTxt.Text));
            tabBarsControllers.SelectedIndex = 1;
            tabBarsControllers.SelectedIndex = 0;
            editInformationCheckBox.Checked = false;
            MessageBox.Show("Personal information updated successfuly.");
        }

        private void aboutToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
        }

        private void priceRangeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(priceRangeCheckBox.Checked == false)
            {
                rangeFromTxt.Text = "";
                rangeToTxt.Text = "";
            }
        }

        private void clearCartBtn_Click(object sender, EventArgs e)
        {
            cartDataGrid.Rows.Clear();
            itemsTotalCostList.Clear();
            quantitiesList.Clear();
            cartList.Clear();
        }
    }
}
