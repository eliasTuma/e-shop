using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ServiceModel;

namespace eShopClient
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public partial class Login : Form, eShopServiceReference.ILoginServiceCallback
    {

        delegate void updateLoginResponse(string reponseMessage);
        delegate void closeCurrentFormDelegate();
        public delegate void updateGUIDelegate();
        public updateGUIDelegate updateGUI;

        public Login()
        {
            InitializeComponent();
        }

        // Set login reponse message in label using invoke
        private void SetResponse(string reponseMessage)
        {
            if (this.resultLbl.InvokeRequired)
            {
                updateLoginResponse u = new updateLoginResponse(SetResponse);
                this.BeginInvoke(u, new object[] { reponseMessage });
            }
            else
            {
                this.resultLbl.Text = reponseMessage;
            }
        }

        public void ResponseToLogin(int response)
        {
            string responseMessage = "";
            if (response == 1)
            {
                responseMessage = "Login Successfully";
                MainForm.usernameLogedIn = usernameTxt.Text;
                updateGUI();
                CloseCurrentForm();
            }
            else if(response == -2)
            {
                responseMessage = "User already logged in !";
            }
            else
            {
                responseMessage = "Bad username or password !";
            }
                

            SetResponse(responseMessage);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            InstanceContext instanceContext = new InstanceContext(this);
            string username = usernameTxt.Text;
            string password = passwordTxt.Text;
            eShopServiceReference.LoginServiceClient client = new eShopServiceReference.LoginServiceClient(instanceContext);
            client.Login(usernameTxt.Text, passwordTxt.Text);
            client.Close();
           // MessageBox.Show("asdasd");
        }


        private void signupBtn_Click(object sender, EventArgs e)
        {
            SignUp newForm = new SignUp();
            newForm.ShowDialog();
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

        public void ResponseToLogout()
        {
            throw new NotImplementedException();
        }
    }
}
