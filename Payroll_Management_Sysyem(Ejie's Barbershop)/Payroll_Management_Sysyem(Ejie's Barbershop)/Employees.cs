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
    public partial class Employees : Form
    {

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\OneDrive\Documents\PayrollDB.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False");
        public Employees()
        {
            InitializeComponent();
            ShowEmployee();
        }



        private void ShowEmployee()
        {
                                        Con.Open();
                                        string query = "SELECT * FROM EmployeeTbl";
                                        SqlDataAdapter sda = new SqlDataAdapter(query, Con);
                                        SqlCommandBuilder builder = new SqlCommandBuilder(sda);
                                        var ds = new DataSet();
                                        sda.Fill(ds);
                                        EmployeeDGV.DataSource = ds.Tables[0];  
                                        Con.Close();

                                        MaximizeDGV();
                                    }

                                    private void MaximizeDGV()
                                    {

                                        EmployeeDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                                        EmployeeDGV.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
                                        EmployeeDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
















        private void Save_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || AddressTb.Text == "" || EmpContNumTb.Text == "" || EmpBaseSalTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("insert into EmployeeTbl (EmpName, EmpAddress, EmpContactNum, EmpBasSal) values (@EN, @EA, @ECN, @EBS)", Con);
                    cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                    cmd.Parameters.AddWithValue("@EA", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@ECN", EmpContNumTb.Text);
                    cmd.Parameters.AddWithValue("@EBS", EmpBaseSalTb.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee Saved");
                    Con.Close();

                    ShowEmployee();

                    EmpNameTb.Clear();
                    AddressTb.Clear();
                    EmpContNumTb.Clear();
                    EmpBaseSalTb.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    


        private void Edit_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || AddressTb.Text == "" || EmpContNumTb.Text == "" || EmpBaseSalTb.Text == "")
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Update EmployeeTbl Set EmpName=@EN, EmpAddress=@EA, EmpContactNum=@ECN, EmpBasSal=@EBS where EmpId=@EmpKey", Con);
                    cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                    cmd.Parameters.AddWithValue("@EA", AddressTb.Text);
                    cmd.Parameters.AddWithValue("@ECN", EmpContNumTb.Text);
                    cmd.Parameters.AddWithValue("@EBS", EmpBaseSalTb.Text);
                    cmd.Parameters.AddWithValue("@EmpKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee Updated");
                    Con.Close();

                    ShowEmployee();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }




        private void Delete_Click(object sender, EventArgs e)
        {
            if (Key == 0)
            {
                MessageBox.Show("Select an employee to delete.");
            }
            else
            {
                try
                {
                    Con.Open();
                    SqlCommand cmd = new SqlCommand("Delete from EmployeeTbl Where EmpId=@EmpKey", Con);
                    cmd.Parameters.AddWithValue("@EmpKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee Deleted");
                    Con.Close();

                    ShowEmployee();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }




        int Key = 0;
        private void EmployeeDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex >= 0)
                {
     
                    DataGridViewRow row = EmployeeDGV.Rows[e.RowIndex];


                    EmpNameTb.Text = row.Cells[1].Value?.ToString() ?? "";
                    AddressTb.Text = row.Cells[2].Value?.ToString() ?? "";
                    EmpContNumTb.Text = row.Cells[3].Value?.ToString() ?? "";
                    EmpBaseSalTb.Text = row.Cells[4].Value?.ToString() ?? "";


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


                            private void DTR_Click(object sender, EventArgs e)
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












        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Employees_Load(object sender, EventArgs e)
        {

        }

    }
}
