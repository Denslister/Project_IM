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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
            CountEmployees();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\OneDrive\Documents\PayrollDB.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False");

        private void CountEmployees()
        {
            Con.Open();
            SqlDataAdapter sda = new SqlDataAdapter("Select Count (*)  from EmployeeTbl", Con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            EmpLbl.Text = dt.Rows[0][0].ToString();
            Con.Close();
        }







        private void label5_Click(object sender, EventArgs e)
        {

        }







        private void Employees_Click(object sender, EventArgs e)
        {
            Employees home = new Employees();
            home.Show();
            this.Hide();
        }

        private void Dtr_Click(object sender, EventArgs e)
        {
            DTR home = new DTR();
            home.Show();
            this.Hide();
        }

        private void Bonus_Click(object sender, EventArgs e)
        {
            Bonus home = new Bonus();
            home.Show();
            this.Hide();
        }


        private void Advance_Click(object sender, EventArgs e)
        {
           Advance home = new Advance();
            home.Show();
            this.Hide();
        }




        private void Payroll_Click(object sender, EventArgs e)
        {
            Payroll home = new Payroll();
            home.Show();
            this.Hide();
        }














        private void Salary_Label_Click(object sender, EventArgs e)
        {

        }

        private void Home_Load(object sender, EventArgs e)
        {

        }

        private void DTR_Label_Click(object sender, EventArgs e)
        {

        }

    }
}
