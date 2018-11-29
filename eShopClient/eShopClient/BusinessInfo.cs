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
    public partial class BusinessInfo : Form
    {
        private string businessName;
        private string businessAddress;

        private delegate void updateTextBoxes();
        public delegate void updateBusinessInformation(string name, string address);
        public updateBusinessInformation updateDelegate;

        public BusinessInfo()
        {
            InitializeComponent();
        }

        public BusinessInfo(string _name, string _address)
        {
            InitializeComponent();
            this.businessName = _name;
            this.businessAddress = _address;
        }

        private void BusinessInfo_Load(object sender, EventArgs e)
        {
            insertInfoIntoTextBoxes();
        }

        private void insertInfoIntoTextBoxes()
        {
            if (this.InvokeRequired)
            {
                updateTextBoxes u = new updateTextBoxes(insertInfoIntoTextBoxes);
                this.BeginInvoke(u);
            }
            else
            {
                businessAddressTxt.Text = this.businessAddress;
                businessNameTxt.Text = this.businessName;
            }
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            updateDelegate(businessNameTxt.Text, businessAddressTxt.Text);
            MessageBox.Show("Updated Successfuly");
            this.Close();
        }
    }
}
