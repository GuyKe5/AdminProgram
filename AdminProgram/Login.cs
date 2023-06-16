using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Windows.Forms;

namespace AdminProgram
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            ApiService apiService = new ApiService();
            User user = await apiService.GetUserData(username, password);

            if (user != null)
            {
                if (user.isAdmin)
                {
                    AdminPage adminPage = new AdminPage();
                    adminPage.Show();

                    this.Hide();
                }
                else
                {
                    // User is not an admin, do something else
                    MessageBox.Show("Login successful for non-admin . you need to be an admin to entery the system");
                }
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

    }
  
}
