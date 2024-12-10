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
    public partial class DTR : Form
    {

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\OneDrive\Documents\PayrollDB.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False");
        public DTR()
        {
            InitializeComponent();
            ShowDTR();
            GetEmployees();
        }



        private void ShowDTR()
        {
            Con.Open();
            string query = "SELECT * FROM DtrTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            DtrDGV.DataSource = ds.Tables[0];
            Con.Close();

            MaximizeDGV();
        }

        private void MaximizeDGV()
        {

            DtrDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            DtrDGV.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            DtrDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }


        private void GetEmployees()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("Select * from EmployeeTbl", Con);
                SqlDataReader Rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("EmpId", typeof(int));
                dt.Load(Rdr);
                EmpCB.ValueMember = "EmpId"; 
                EmpCB.DisplayMember = "EmpId"; 
                EmpCB.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                Con.Close();
            }
        }


        private void GetEmployeeName()
        {
            Con.Open();
            string Query = "Select * from EmployeeTbl where EmpId=" + EmpCB.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(Query, Con);
            DataTable dt = new DataTable(); 
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                EmpNameTb.Text = dr["EmpName"].ToString();
            }
            Con.Close();
        }







        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || AMhoursWorkTb.Text == "" || PMhoursWorkTb.Text == "" || EmpCB.SelectedValue == null)
            {
                MessageBox.Show("Missing Information");
                return;
            }

            try
            {

                if (!decimal.TryParse(AMhoursWorkTb.Text, out decimal amHours) ||
                    !decimal.TryParse(PMhoursWorkTb.Text, out decimal pmHours))
                {
                    MessageBox.Show("Invalid input for hours worked. Please enter numeric values.");
                    return;
                }


                string Period = DtrPeriod.Value.ToString("dddd, dd MMMM yyyy");


                int EmpId = Convert.ToInt32(EmpCB.SelectedValue);


                Con.Open();


                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO DtrTbl (EmpId, EmpName, AM_HoursWorked, PM_HoursWorked, Period) " +
                    "VALUES (@EmpId, @EmpName, @AMHW, @PMHW, @Period)", Con);

    
                cmd.Parameters.AddWithValue("@EmpId", EmpId); 
                cmd.Parameters.AddWithValue("@EmpName", EmpNameTb.Text); 
                cmd.Parameters.AddWithValue("@AMHW", amHours); 
                cmd.Parameters.AddWithValue("@PMHW", pmHours); 
                cmd.Parameters.AddWithValue("@Period", Period); 


                cmd.ExecuteNonQuery();


                MessageBox.Show("DTR Saved");


                Con.Close();


                ShowDTR();


                EmpNameTb.Clear();
                AMhoursWorkTb.Clear();
                PMhoursWorkTb.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }




        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (EmpNameTb.Text == "" || AMhoursWorkTb.Text == "" || PMhoursWorkTb.Text == "" || EmpCB.SelectedValue == null)
            {
                MessageBox.Show("Missing Information");
                return;
            }

            if (Key == 0) 
            {
                MessageBox.Show("Select a record to update.");
                return;
            }

            try
            {

                string Period = DtrPeriod.Value.ToString("dddd, dd MMMM yyyy");


                Con.Open();


                SqlCommand cmd = new SqlCommand(
                    "UPDATE DtrTbl SET EmpId=@EmpId, EmpName=@EmpName, AM_HoursWorked=@AMHW, PM_HoursWorked=@PMHW, Period=@Period WHERE DtrId=@DtrKey",
                    Con);


                cmd.Parameters.AddWithValue("@EmpId", EmpCB.SelectedValue); 
                cmd.Parameters.AddWithValue("@EmpName", EmpNameTb.Text);
                cmd.Parameters.AddWithValue("@AMHW", AMhoursWorkTb.Text); 
                cmd.Parameters.AddWithValue("@PMHW", PMhoursWorkTb.Text); 
                cmd.Parameters.AddWithValue("@Period", Period); 
                cmd.Parameters.AddWithValue("@DtrKey", Key); 


                cmd.ExecuteNonQuery();


                MessageBox.Show("DTR Updated Successfully");


                Con.Close();

                ShowDTR();

                EmpNameTb.Clear();
                AMhoursWorkTb.Clear();
                PMhoursWorkTb.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);

                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }
        }




        private void DeleteBtn_Click(object sender, EventArgs e)
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
                    SqlCommand cmd = new SqlCommand("Delete from DtrTbl Where DtrId=@DtrKey", Con);
                    cmd.Parameters.AddWithValue("@DtrKey", Key);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("DTR Deleted");
                    Con.Close();

                    ShowDTR();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }



        int Key = 0;
        private void DtrDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex >= 0)
                {

                    DataGridViewRow row = DtrDGV.Rows[e.RowIndex];


                    EmpCB.Text = row.Cells[1].Value?.ToString() ?? "";
                    EmpNameTb.Text = row.Cells[2].Value?.ToString() ?? "";
                    AMhoursWorkTb.Text = row.Cells[3].Value?.ToString() ?? "";
                    PMhoursWorkTb.Text = row.Cells[4].Value?.ToString() ?? "";
                    DtrPeriod.Text = row.Cells[5].Value?.ToString() ?? "";


                    Key = row.Cells[0].Value != null ? Convert.ToInt32(row.Cells[0].Value) : 0;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message);
            }
        }





        private void EmpCB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetEmployeeName();
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





















        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DTR_Load(object sender, EventArgs e)
        {

        }

        private void EmpCB_Click(object sender, EventArgs e)
        {

        }

    }
}
