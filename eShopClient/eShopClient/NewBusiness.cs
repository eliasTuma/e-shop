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
    public partial class NewBusiness : Form
    {
       
        public NewBusiness()
        {
            InitializeComponent();
        }
        public NewBusiness(string ownerId)
        {
            InitializeComponent();
            this.ownerIdTxt.Text = ownerId;
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            int businessId, ownerId;
            InstanceContext instanceContext = new InstanceContext(this);
            eShopServiceReference.SignUpBusinessServiceClient service = new eShopServiceReference.SignUpBusinessServiceClient();
            if (!CheckIfTextBoxesNotNull())
            {
                MessageBox.Show("Business information can not be empty.", "Null information are not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                businessId = Int32.Parse(businessIdTxt.Text);
                ownerId = Int32.Parse(ownerIdTxt.Text);
                service.CheckBusinessIdAvalibility(businessId);
                int result = service.SignUpBusiness(Int32.Parse(ownerIdTxt.Text), Int32.Parse(businessIdTxt.Text),
                                                    businessNameTxt.Text, businessAddressTxt.Text);
                if (result == 1)
                {
                    MessageBox.Show("Business was successfuly registered.", "Sign- up completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Business sign up failed, please try again.", "Sign-up failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Business id must contain only numbers.", "Invalid business id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }        
        }
        private bool CheckIfTextBoxesNotNull()
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                if (tb.Text == "")
                    return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            eShopServiceReference.SignUpBusinessServiceClient client = new eShopServiceReference.SignUpBusinessServiceClient();
            int result = client.CheckBusinessIdAvalibility(Int32.Parse(businessIdTxt.Text));
            if(result == 1)
            {
                MessageBox.Show("Business id already exist, please choose different one.", "Invalid business id", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Business id not in use.", "Valid business id", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
