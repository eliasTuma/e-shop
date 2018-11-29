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
    public partial class EditProduct : Form
    {
        int bId, pId,_index;
        string pName, pKeys;
        float pPrice;

        public delegate void updateProductInfoDelegate(int selectedIndex, int businessId, int productId, string productName, float productPrice, string productKeys);
        public updateProductInfoDelegate updateProductInfo;

        public EditProduct()
        {
            InitializeComponent();
        }

        public EditProduct(int selectedIndex, int businessId, int productId, string productName, float productPrice, string productKeys)
        {
            InitializeComponent();
            bId = businessId;
            pId = productId;
            pName = ProductName;
            pPrice = productPrice;
            _index = selectedIndex;
            pKeys = productKeys;

            productIdTxt.Text = productId.ToString();
            productNameTxt.Text = productName;
            productPriceTxt.Text = pPrice.ToString();
            productKeysTxt.Text = pKeys;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pName = productNameTxt.Text;
            pKeys = productKeysTxt.Text;
            try
            {
                pId = Int32.Parse(productIdTxt.Text);
                if (pId < 0)
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
                pPrice = float.Parse(productPriceTxt.Text);
                if (pPrice < 0)
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

            MainForm.mainActivity.UpdateProductInfo(bId, pId, pName, pPrice, pKeys);
            MessageBox.Show("Product updated successfuly.", "Updated Successfuly", MessageBoxButtons.OK, MessageBoxIcon.Information);
            updateProductInfo(_index, bId, pId, pName, pPrice, pKeys);
            this.Close();
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
