using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gtavvehicles
{
    public partial class ConnectionCredentials : Form
    {
        public ConnectionCredentials()
        {
            InitializeComponent();
        }

        private void ConnectionCredentials_Load(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // If the username or password is empty then show an error message
            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter a username and password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmMain.db.ConnectionString = $"server=vulpecula.flaviopereira.digital;user={username};password={password};database=fivem_opadrinho";

            try
            {
                frmMain.db.Open();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
