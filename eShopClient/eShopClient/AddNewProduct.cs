using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eShopClient
{
    public partial class AddNewProduct : Form
    {
        public delegate void updateProductGrid(int productId, string productName, float price, string productKeys);
        public updateProductGrid updateProductsList;
        private int businessId;

        public AddNewProduct()
        {
            InitializeComponent();
        }

        public AddNewProduct(int _id)
        {
            InitializeComponent();
            businessId = _id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int productId, result;
            string productName = productNameTxt.Text, productKeys = productKeysTxt.Text;
            float price;

            try
            {
                productId = Int32.Parse(productIdTxt.Text);
                if (productId < 0)
                {
                    MessageBox.Show("Product id must be greater than 0.", "Invalid product id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Product id must contain numbers only.", "Invalid product id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                price = float.Parse(productPriceTxt.Text);
                if(price < 0)
                {
                    MessageBox.Show("Product price must be greater than 0.", "Invalid product price", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Product price must contain numbers only.", "Invalid product price", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!checkIfTextBoxesNotNull())
            {
                MessageBox.Show("Please fill out all the information", "Information messing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            result = MainForm.mainActivity.AddProductForBusiness(businessId, productId, productName, price, productKeys);
            if(result == -1)
            {
                MessageBox.Show("ProductId already in use, change it and try again","Invalid ProductId", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("Product added successfuly");
                updateProductsList(productId, productName, price, productKeys);
                this.Close();
            }
            
        }

        private bool checkIfTextBoxesNotNull()
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                if (tb.Text == "")
                    return false;
            }
            return true;
        }
    }

    
}
