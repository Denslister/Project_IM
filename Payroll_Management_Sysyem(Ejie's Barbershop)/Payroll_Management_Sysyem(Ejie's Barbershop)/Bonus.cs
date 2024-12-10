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
    public partial class Bonus : Form
    {
        public Bonus()
        {
            InitializeComponent();
            ShowBonus();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\OneDrive\Documents\PayrollDB.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False");


        private void ShowBonus()
        {
            Con.Open();
            string query = "SELECT * FROM BonusTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
          BonusDGV .DataSource = ds.Tables[0];
            Con.Close();

            MaximizeDGV();
        }

        private void MaximizeDGV()
        {

            BonusDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            BonusDGV.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            BonusDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }





        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (TOBTb.Text == "" || BAmountTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into BonusTbl (BName, BAmount) values (@BN, @BA)", Con);
                    cmd.Parameters.AddWithValue("@BN",TOBTb.Text);
                    cmd.Parameters.AddWithValue("@BA", BAmountTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bonus Saved");
                    Con.Close();

                    ShowBonus();

                    TOBTb.Clear();
                    BAmountTb.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }





        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (TOBTb.Text == "" || BAmountTb.Text == "" )
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update BonusTbl Set BName=@BN, BAmount=@BA where BId=@BKey", Con);
                    cmd.Parameters.AddWithValue("@BN", TOBTb.Text);
                    cmd.Parameters.AddWithValue("@BA", BAmountTb.Text);
                    cmd.Parameters.AddWithValue("@BKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bonus Updated");
                    Con.Close();

                    ShowBonus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }




        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select an bonus to delete.");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from BonusTbl Where BId=@BKey", Con);
                    cmd.Parameters.AddWithValue("@BKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Bonus Deleted");
                    Con.Close();

                    ShowBonus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }







        int Key = 0 ;
        private void BonusDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex >= 0)
                {

                    DataGridViewRow row = BonusDGV.Rows[e.RowIndex];


                   TOBTb.Text = row.Cells[1].Value?.ToString() ?? "";
                    BAmountTb.Text = row.Cells[2].Value?.ToString() ?? "";


                    Key = row.Cells[0].Value != null ? Convert.ToInt32(row.Cells[0].Value) : 0;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message);
            }
        }




        private void Home_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
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















        private void Allowance_Load(object sender, EventArgs e)
        {

        }

    }
}
