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
using System.ServiceModel.Description;

namespace eShopHost
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool isServiceRunning = false;
        ServiceHost host;

        private void Form1_Load(object sender, EventArgs e)
        {
            host = new ServiceHost(typeof(eShopService.eShopServices));
            try
            {
                host.Open();
                label1.Text = "Service is running";
                isServiceRunning = true;
            }
            catch (Exception)
            {

                label1.Text = "Service is NOT running";
                isServiceRunning = false;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isServiceRunning)
                host.Close();
        }
    }
}
