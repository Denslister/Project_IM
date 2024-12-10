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
    public partial class Advance : Form
    {
        public Advance()
        {
            InitializeComponent();
            ShowAdvance();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\OneDrive\Documents\PayrollDB.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False");


        private void ShowAdvance()
        {
            Con.Open();
            string query = "SELECT * FROM AdvanceTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            AdvanceDGV.DataSource = ds.Tables[0];
            Con.Close();

            MaximizeDGV();
        }

        private void MaximizeDGV()
        {

            AdvanceDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            AdvanceDGV.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            AdvanceDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }





        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (TypeOfAdvTb.Text == "" || AdvAmountTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into AdvanceTbl (AdvName, AdvAmount) values (@AN, @AA)", Con);
                    cmd.Parameters.AddWithValue("@AN", TypeOfAdvTb.Text);
                    cmd.Parameters.AddWithValue("@AA", AdvAmountTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Advance Saved");
                    Con.Close();

                    ShowAdvance();

                    TypeOfAdvTb.Clear();
                    AdvAmountTb.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }



        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (TypeOfAdvTb.Text == "" || AdvAmountTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update AdvanceTbl Set AdvName=@AN, AdvAmount=@AA where AdvId=@AdvKey", Con);
                    cmd.Parameters.AddWithValue("@AN", TypeOfAdvTb.Text);
                    cmd.Parameters.AddWithValue("@AA", AdvAmountTb.Text);
                    cmd.Parameters.AddWithValue("@AdvKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Advance Updated");
                    Con.Close();

                    ShowAdvance();
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
                MessageBox.Show("Select an Advance to delete.");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from AdvanceTbl Where AdvId=@AdvKey", Con);
                    cmd.Parameters.AddWithValue("@AdvKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Advance Deleted");
                    Con.Close();

                    ShowAdvance();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        int Key = 0;
        private void AdvanceDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex >= 0)
                {

                    DataGridViewRow row = AdvanceDGV.Rows[e.RowIndex];


                    TypeOfAdvTb.Text = row.Cells[1].Value?.ToString() ?? "";
                    AdvAmountTb.Text = row.Cells[2].Value?.ToString() ?? "";


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

        private void Bonus_Click(object sender, EventArgs e)
        {
           Bonus home = new Bonus();
            home.Show();
            this.Hide();
        }


        private void Payroll_Click(object sender, EventArgs e)
        {
            Payroll home = new Payroll();
            home.Show();
            this.Hide();
        }














        private void Deductory_Load(object sender, EventArgs e)
        {

        }


    }
}
