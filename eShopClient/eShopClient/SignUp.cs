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
    [CallbackBehavior(UseSynchronizationContext = false)]
    public partial class SignUp : Form,eShopServiceReference.ISignUpServiceCallback
    {
        private int idAvailablity = -1, usernameAvailablity = -1;

        delegate void updateUsernameAvabilityResponse(string responseMessage,int responseType);
        delegate void updateIdAvabilityResponse(string responseMessage, int responseType);
        delegate void closeCurrentFormDelegate();

        private void setUsernameAvailabilityResponse(string responseMessage,int responseType)
        {
            if (this.usernameAvailablityLbl.InvokeRequired)
            {
                updateUsernameAvabilityResponse u = new updateUsernameAvabilityResponse(setUsernameAvailabilityResponse);
                this.BeginInvoke(u, new object[] { responseMessage, responseType });
            }
            else
            {
                if(responseType == 1)
                {
                    this.usernameAvailablityLbl.ForeColor = Color.Red;
                    this.usernameAvailablity = 0;
                }
                else
                {
                    this.usernameAvailablityLbl.ForeColor = Color.Green;
                    this.usernameAvailablity = 1;
                }
                this.usernameAvailablityLbl.Text = responseMessage;
            }
        }

        /// <summary>
        /// Show the result in label using invoke
        /// </summary>
        /// <param name="responseMessage">Response Message</param>
        /// <param name="responseType">Response Type</param>
        private void setIdAvailabilityResponse(string responseMessage, int responseType)
        {
            if (this.idAvailabilityLbl.InvokeRequired)
            {
                updateIdAvabilityResponse u = new updateIdAvabilityResponse(setIdAvailabilityResponse);
                this.BeginInvoke(u, new object[] { responseMessage, responseType });
            }
            else
            {
                if(responseType == 1)
                {
                    this.idAvailabilityLbl.ForeColor = Color.Red;
                    this.idAvailablity = 0;
                }
                else
                {
                    this.idAvailabilityLbl.ForeColor = Color.Green;
                    this.idAvailablity = 1;
                }

                this.idAvailabilityLbl.Text = responseMessage;
            }
        }
        public SignUp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Server response for checking id availabilty
        /// </summary>
        /// <param name="response">Result</param>
        public void IdAvailabilityResponse(int response)
        {
            string responseMessage = "";
            switch (response)
            {
                case 1:
                    responseMessage = "id is in use.";
                    break;
                case -1:
                    responseMessage = "id is available.";
                    break;
            }
            setIdAvailabilityResponse(responseMessage, response);
        }

        /// <summary>
        /// Server response to sign up request
        /// </summary>
        /// <param name="response">Response</param>
        public void ResponseToSignUp(int response)
        {
            if(response == 1)
            {
                MessageBox.Show("Sign-up completed. You may login now.", "Sign-up Successfuly", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CloseCurrentForm();
            }
            else
            {
                MessageBox.Show("Sign-up failed, check the information", "Sign-up failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseCurrentForm()
        {
            if (this.InvokeRequired)
            {
                closeCurrentFormDelegate u = new closeCurrentFormDelegate(CloseCurrentForm);
                BeginInvoke(u);
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// Server response for checking username availabilty
        /// </summary>
        /// <param name="response">Result</param>
        public void UsernameAvailabilityResponse(int response)
        {
            string responseMessage = "";
            switch (response)
            {
                case 1:
                    responseMessage = "Username is in use.";
                    break;
                case -1:
                    responseMessage = "Username is available.";
                    break;
            }
            setUsernameAvailabilityResponse(responseMessage,response);
        }

        /// <summary>
        /// Request from server to check username in the database
        /// </summary>
        /// <param name="sender">Check username availability button</param>
        /// <param name="e">Button_Click</param>
        private void usernameAvailabilityBtn_Click(object sender, EventArgs e)
        {
            usernameTxt.Text = usernameTxt.Text.Trim();
            if (usernameTxt.Text == "")
            {
                MessageBox.Show("Username property can't be null !", "Information Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            InstanceContext instanceContext = new InstanceContext(this);
            eShopServiceReference.SignUpServiceClient client = new eShopServiceReference.SignUpServiceClient(instanceContext);
            client.CheckUsernameAvailability(usernameTxt.Text);
            client.Close();       
        }

        private void submitBtn_Click(object sender, EventArgs e)
        {
            if (!checkIfOnlyNumric())
            {
                return;
            }

            if (!checkIfTextBoxesNotNull())
            {
                MessageBox.Show("You need to enter all the information in order to sign up", "Information is missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // The information is valid, request sign-up
            InstanceContext instanceContext = new InstanceContext(this);
            eShopServiceReference.SignUpServiceClient signUpClient = new eShopServiceReference.SignUpServiceClient(instanceContext);
            signUpClient.SignUp(usernameTxt.Text, passwordTxt.Text, Int32.Parse(idTxt.Text),
                fNameTxt.Text, lNameTxt.Text, addressTxt.Text, phoneTxt.Text, Int32.Parse(balanceTxt.Text));
            signUpClient.Close();
        }

        /// <summary>
        /// Checks if id and balance text fields contains only numbers
        /// </summary>
        /// <returns>True if the values are numbers only, False otherwise</returns>
        private bool checkIfOnlyNumric()
        {
            try
            {
                int tmp = Int32.Parse(idTxt.Text);
            }
            catch (Exception)
            {

                MessageBox.Show("Id field must contain only numbers", "Invalid Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            try
            {
                int tmp2 = Int32.Parse(balanceTxt.Text);
                if(tmp2 < 0)
                {
                    MessageBox.Show("Blanace must be greater than 0", "Invalid balance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Balance field must contain only numbers", "Invalid Id", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if any of the textboxes is null or empty
        /// </summary>
        /// <returns>True - in case of empty textboxes. False - otherwise</returns>
        private bool checkIfTextBoxesNotNull()
        {
            foreach (TextBox tb in this.Controls.OfType<TextBox>())
            {
                tb.Text.Trim();
                if (tb.Text == "")
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Checks the username and id availability before signing up
        /// </summary>
        /// <returns>True - if they are not in use. False otherwise</returns>
        private bool checkIfValidUsernameAndId()
        {
            bool validInformation = true;
            switch (idAvailablity)
            {
                // The id availability was not checked
                case -1:
                    InstanceContext instanceContext = new InstanceContext(this);
                    eShopServiceReference.SignUpServiceClient client = new eShopServiceReference.SignUpServiceClient(instanceContext);
                    client.CheckIdAvailability(Int32.Parse(idTxt.Text));
                    // The id is already in use
                    if (idAvailablity == 0)
                    {
                        IdAvailabilityResponse(1);
                        validInformation = false;
                    }
                    break;
                // The id was checked and already in use
                case 0:
                    IdAvailabilityResponse(1);
                    validInformation = false;
                    break;
            }
            switch (usernameAvailablity)
            {
                // The username availability was not checked
                case -1:
                    InstanceContext instanceContext = new InstanceContext(this);
                    eShopServiceReference.SignUpServiceClient client = new eShopServiceReference.SignUpServiceClient(instanceContext);
                    client.CheckUsernameAvailability(usernameTxt.Text);
                    // The username is already in use
                    if (usernameAvailablity == 0)
                    {
                        UsernameAvailabilityResponse(1);
                        validInformation = false;
                    }
                    break;
                // The username was checked and already in use
                case 0:
                    validInformation = false;
                    break; 
            }

            return validInformation;

        }
    }
}
