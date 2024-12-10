using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Payroll_Management_Sysyem_Ejie_s_Barbershop_
{
    public partial class Login : Form
    {

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\OneDrive\Documents\PayrollManageDB.mdf;Integrated Security=True;Connect Timeout=30");
        public Login()
        {
            InitializeComponent();
            PasswordTb.PasswordChar = '*';
            UsernameTb.KeyDown += UsernameTb_KeyDown;
            PasswordTb.KeyDown += PasswordTb_KeyDown;
            this.Load += Login_Load;

            UsernameTb.TabIndex = 0;
            PasswordTb.TabIndex = 1;
            LoginBtn.TabIndex = 2;
        }

       
        
        
        private void Login_Load(object sender, EventArgs e)
        {
            this.BeginInvoke((Action)(() => UsernameTb.Focus()));
        }

        
        
        
        
        private void PasswordTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                
                LoginBtn.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true; 
            }
        }

       
        
        
        
        
        
        
        private void UsernameTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                LoginBtn.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true; 
            }
        }

        
        
        
        
        
        
        
        private void LoginBtn_Click(object sender, EventArgs e)
        {

            string username = UsernameTb.Text;
            string password = PasswordTb.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter username or  password.");
                return;
            }

            try
            {

                Con.Open();


                string query = "SELECT COUNT(1) FROM LoginTbl WHERE Username = @username AND Password = @password";
                SqlCommand cmd = new SqlCommand(query, Con);


                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);


                int count = (int)cmd.ExecuteScalar();

                if (count == 1)
                {

                    Home homePage = new Home();
                    homePage.Show();


                    this.Hide();
                }
                else
                {

                    MessageBox.Show("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {

                Con.Close();
            }
        }

      
        
        
        
        
        
        
        private void PasswordTb_KeyDown1(object sender, KeyEventArgs e)
        {
            if (sender is null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e.KeyCode == Keys.Enter)
            {
          
                LoginBtn.PerformClick();
                e.Handled = true;
                e.SuppressKeyPress = true; 
            }
        }

        private void Login_Load_1(object sender, EventArgs e)
        {

        }
    }
}

