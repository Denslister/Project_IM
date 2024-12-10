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
using System.Globalization;

namespace Payroll_Management_Sysyem_Ejie_s_Barbershop_
{
    public partial class Payroll : Form
    {

        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\OneDrive\Documents\PayrollDB.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=False");
        public Payroll()
        {
            InitializeComponent();
            ShowPayroll();
            GetEmployees();
            GetDTR();
            GetBonus();
        }

        private void ShowPayroll()
        {
            Con.Open();
            string query = "SELECT * FROM PayrollTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            PayrollDGV.DataSource = ds.Tables[0];
            Con.Close();

            MaximizeDGV();
        }

        private void MaximizeDGV()
        {

           PayrollDGV.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            PayrollDGV.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
            PayrollDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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


        private void GetBonus()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("Select * from BonusTbl", Con);
                SqlDataReader Rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("BName", typeof(string));
                dt.Load(Rdr);
                BonusCB.ValueMember = "BName";
                BonusCB.DisplayMember = "BName";
                BonusCB.DataSource = dt;
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





        private void GetDTR()
        {
            try
            {
                if (EmpCB.SelectedValue == null)
                {
                    MessageBox.Show("Please select an employee first.");
                    return;
                }

                Con.Open();

                string query = "SELECT * FROM DtrTbl WHERE EmpId = @EmpId";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@EmpId", EmpCB.SelectedValue);

                SqlDataReader Rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("DtrId", typeof(int));
                dt.Load(Rdr);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("No DTR records found for the selected employee.");
                    DtrCB.DataSource = null;
                }
                else
                {
                    DtrCB.ValueMember = "DtrId";
                    DtrCB.DisplayMember = "DtrId";
                    DtrCB.DataSource = dt;
                }
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


        private void GetDtrData()
        {
            if (DtrCB.SelectedValue == null)
            {
                MessageBox.Show("Please select a valid DTR record.");
                return;
            }

            try
            {

                Con.Open();

                string query = "SELECT AM_HoursWorked, PM_HoursWorked FROM DtrTbl WHERE DtrId = @DtrId";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@DtrId", DtrCB.SelectedValue);
                SqlDataReader Rdr = cmd.ExecuteReader();

                if (Rdr.Read())
                {
                    AM_HoursWorkedTb.Text = Rdr["AM_HoursWorked"].ToString();
                    PM_HoursWorkedTb.Text = Rdr["PM_HoursWorked"].ToString();
                }
                else
                {
                    MessageBox.Show("DTR record not found.");
                    AM_HoursWorkedTb.Clear();
                    PM_HoursWorkedTb.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching DTR data: " + ex.Message);
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
                BasSalaryTb.Text = dr["EmpBasSal"].ToString();
            }
            Con.Close();
        }


        private void GetBAmount()
        {
            if (BonusCB.SelectedValue == null)
            {
                MessageBox.Show("Please select a valid bonus.");
                return;
            }

            try
            {

                Con.Open();


                string query = "SELECT BAmount FROM BonusTbl WHERE BName = @BName";
                SqlCommand cmd = new SqlCommand(query, Con);


                cmd.Parameters.AddWithValue("@BName", BonusCB.SelectedValue);


                SqlDataReader Rdr = cmd.ExecuteReader();

                if (Rdr.Read())
                {

                    BonusTb.Text = Rdr["BAmount"].ToString();
                }
                else
                {
                    MessageBox.Show("Bonus not found.");
                    BonusTb.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching bonus amount: " + ex.Message);
            }
            finally
            {

                Con.Close();
            }
        }



        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmpCB.Text) ||
           string.IsNullOrWhiteSpace(EmpNameTb.Text) ||
           string.IsNullOrWhiteSpace(BasSalaryTb.Text) ||
           string.IsNullOrWhiteSpace(AM_HoursWorkedTb.Text) ||
           string.IsNullOrWhiteSpace(PM_HoursWorkedTb.Text) ||
           string.IsNullOrWhiteSpace(BalanceTb.Text) ||
           string.IsNullOrWhiteSpace(AllowDTP.Text) ||
           string.IsNullOrWhiteSpace(PaySalaryTb.Text))
            {
                MessageBox.Show("Missing critical information. Please fill in all required fields.");
                return;
            }

            try
            {

                decimal bonus = 0;
                decimal advance = 0;

                if (!string.IsNullOrWhiteSpace(BonusTb.Text) && !decimal.TryParse(BonusTb.Text, out bonus))
                {
                    MessageBox.Show("Invalid bonus value. Please enter a valid number.");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(AdvanceTb.Text) && !decimal.TryParse(AdvanceTb.Text, out advance))
                {
                    MessageBox.Show("Invalid advance value. Please enter a valid number.");
                    return;
                }

  
                Con.Open();
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO PayrollTbl (EmpId, EmpName, EmpBasSal, EmpBonus, EmpAdvance, EmpDTRam, EmpDTRpm, EmpBalance, PayPeriod, EmpSalary) " +
                    "VALUES (@EI, @EN, @EBS, @EB, @EA, @EDtrAM, @EDtrPM, @EBAL, @PP, @ES)", Con);


                cmd.Parameters.AddWithValue("@EI", EmpCB.Text);
                cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                cmd.Parameters.AddWithValue("@EBS", decimal.Parse(BasSalaryTb.Text));
                cmd.Parameters.AddWithValue("@EB", bonus);
                cmd.Parameters.AddWithValue("@EA", advance);
                cmd.Parameters.AddWithValue("@EdtrAM", decimal.Parse(AM_HoursWorkedTb.Text));
                cmd.Parameters.AddWithValue("@EdtrPM", decimal.Parse(PM_HoursWorkedTb.Text));
                cmd.Parameters.AddWithValue("@EBAL", decimal.Parse(BalanceTb.Text));
                cmd.Parameters.AddWithValue("@PP", AllowDTP.Text);
                cmd.Parameters.AddWithValue("@ES", decimal.Parse(PaySalaryTb.Text));

                cmd.ExecuteNonQuery();
                Con.Close();


                MessageBox.Show("Payroll record saved successfully.");
                ShowPayroll();

  
                EmpCB.SelectedIndex = -1;
                EmpNameTb.Clear();
                BasSalaryTb.Clear();
                BonusTb.Clear();
                AdvanceTb.Clear();
                AM_HoursWorkedTb.Clear();
                PM_HoursWorkedTb.Clear();
                BalanceTb.Clear();
                AllowDTP.ResetText();
                PaySalaryTb.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }




        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EmpCB.Text) ||
         string.IsNullOrWhiteSpace(EmpNameTb.Text) ||
         string.IsNullOrWhiteSpace(BasSalaryTb.Text) ||
         string.IsNullOrWhiteSpace(AM_HoursWorkedTb.Text) ||
         string.IsNullOrWhiteSpace(PM_HoursWorkedTb.Text) ||
         string.IsNullOrWhiteSpace(BalanceTb.Text) ||
         string.IsNullOrWhiteSpace(AllowDTP.Text) ||
         string.IsNullOrWhiteSpace(PaySalaryTb.Text))
            {
                MessageBox.Show("Missing critical information. Please fill in all required fields.");
                return;
            }

            try
            {

                decimal bonus = 0;
                decimal advance = 0;

                if (!string.IsNullOrWhiteSpace(BonusTb.Text) && !decimal.TryParse(BonusTb.Text, out bonus))
                {
                    MessageBox.Show("Invalid bonus value. Please enter a valid number.");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(AdvanceTb.Text) && !decimal.TryParse(AdvanceTb.Text, out advance))
                {
                    MessageBox.Show("Invalid advance value. Please enter a valid number.");
                    return;
                }


                int hourlyRate = 56;


                int AMHours = string.IsNullOrWhiteSpace(AM_HoursWorkedTb.Text) ? 0 : Convert.ToInt32(AM_HoursWorkedTb.Text);
                int PMHours = string.IsNullOrWhiteSpace(PM_HoursWorkedTb.Text) ? 0 : Convert.ToInt32(PM_HoursWorkedTb.Text);


                int totalHours = AMHours + PMHours;

  
                int salary = (totalHours * hourlyRate) + (int)bonus + (int)advance;


                decimal balance = decimal.Parse(BalanceTb.Text);
                decimal totalBalance = balance - salary;

                if (totalBalance < 0)
                {
                    totalBalance = 0; 
                }

             
                Con.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE PayrollTbl SET EmpName=@EN, EmpBasSal=@EBS, EmpBonus=@EB, EmpAdvance=@EA, " +
                    "EmpDTRam=@EDtrAM, EmpDTRpm=@EDtrPM, EmpBalance=@EBAL, PayPeriod=@PP, EmpSalary=@ES " +
                    "WHERE EmpId=@EmpKey", Con);


                cmd.Parameters.AddWithValue("@EmpKey", EmpCB.Text);
                cmd.Parameters.AddWithValue("@EN", EmpNameTb.Text);
                cmd.Parameters.AddWithValue("@EBS", decimal.Parse(BasSalaryTb.Text));
                cmd.Parameters.AddWithValue("@EB", bonus);
                cmd.Parameters.AddWithValue("@EA", advance); 
                cmd.Parameters.AddWithValue("@EDtrAM", AMHours); 
                cmd.Parameters.AddWithValue("@EDtrPM", PMHours); 
                cmd.Parameters.AddWithValue("@EBAL", totalBalance); 
                cmd.Parameters.AddWithValue("@PP", AllowDTP.Text); 
                cmd.Parameters.AddWithValue("@ES", salary); 

                cmd.ExecuteNonQuery();
                Con.Close();

                MessageBox.Show("Payroll record updated successfully.");
                ShowPayroll();


                EmpCB.SelectedIndex = -1;
                EmpNameTb.Clear();
                BasSalaryTb.Clear();
                BonusTb.Clear();
                AdvanceTb.Clear();
                AM_HoursWorkedTb.Clear();
                PM_HoursWorkedTb.Clear();
                BalanceTb.Clear();
                AllowDTP.ResetText();
                PaySalaryTb.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }












        private void PayrollDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = PayrollDGV.Rows[e.RowIndex];


                    EmpCB.Text = row.Cells["EmpId"].Value?.ToString() ?? "";
                    EmpNameTb.Text = row.Cells["EmpName"].Value?.ToString() ?? "";
                    BasSalaryTb.Text = row.Cells["EmpBasSal"].Value?.ToString() ?? "";
                    BonusTb.Text = row.Cells["EmpBonus"].Value?.ToString() ?? "";
                    AdvanceTb.Text = row.Cells["EmpAdvance"].Value?.ToString() ?? "";
                    AM_HoursWorkedTb.Text = row.Cells["EmpDTRam"].Value?.ToString() ?? "";
                    PM_HoursWorkedTb.Text = row.Cells["EmpDTRpm"].Value?.ToString() ?? "";
                    BalanceTb.Text = row.Cells["EmpBalance"].Value?.ToString() ?? "";
                    AllowDTP.Text = row.Cells["PayPeriod"].Value?.ToString() ?? "";
                    PaySalaryTb.Text = row.Cells["EmpSalary"].Value?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }














        private void DtrCB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetEmployeeName();
            GetDTR();
        }










        int DailyBase = 0, Total = 0, AHours = 0, PHours = 0;
        private void ComputeBtn_Click(object sender, EventArgs e)
        {
            try
            {
    
                int baseSalary = string.IsNullOrWhiteSpace(BasSalaryTb.Text) ? 0 : Convert.ToInt32(BasSalaryTb.Text);
                int AMHours = string.IsNullOrWhiteSpace(AM_HoursWorkedTb.Text) ? 0 : Convert.ToInt32(AM_HoursWorkedTb.Text);
                int PMHours = string.IsNullOrWhiteSpace(PM_HoursWorkedTb.Text) ? 0 : Convert.ToInt32(PM_HoursWorkedTb.Text);
                int bonus = string.IsNullOrWhiteSpace(BonusTb.Text) ? 0 : Convert.ToInt32(BonusTb.Text);
                int advance = string.IsNullOrWhiteSpace(AdvanceTb.Text) ? 0 : Convert.ToInt32(AdvanceTb.Text);


                if (AMHours < 0 || PMHours < 0 || bonus < 0 || advance < 0 || baseSalary < 0)
                {
                    MessageBox.Show("Please ensure all inputs are non-negative numbers.");
                    return;
                }


                int hourlyRate = 56;


                int totalHours = AMHours + PMHours;


                int totalSalary = (totalHours * hourlyRate) + bonus + advance;


                int balance = totalSalary - baseSalary;


                balance = Math.Abs(balance);


                PaySalaryTb.Text = totalSalary.ToString(); 
                BalanceTb.Text = balance.ToString();       

                MessageBox.Show("Computation Successful.");
            }
            catch (Exception ex)
            {

                MessageBox.Show("An error occurred: " + ex.Message);
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


        private void Advance_Click(object sender, EventArgs e)
        {
           Advance home = new Advance();
            home.Show();
            this.Hide();
        }




        private void GeneratePayrollReceipt()
        {
            try
            {

                string employeeName = EmpNameTb.Text;
                string employeeId = EmpCB.Text;
                decimal baseSalary = string.IsNullOrWhiteSpace(BasSalaryTb.Text) ? 0 : decimal.Parse(BasSalaryTb.Text);
                decimal bonus = string.IsNullOrWhiteSpace(BonusTb.Text) ? 0 : decimal.Parse(BonusTb.Text);
                decimal advance = string.IsNullOrWhiteSpace(AdvanceTb.Text) ? 0 : decimal.Parse(AdvanceTb.Text);
                decimal totalSalary = string.IsNullOrWhiteSpace(PaySalaryTb.Text) ? 0 : decimal.Parse(PaySalaryTb.Text);
                decimal balance = string.IsNullOrWhiteSpace(BalanceTb.Text) ? 0 : decimal.Parse(BalanceTb.Text);
                string payPeriod = AllowDTP.Text;


                CultureInfo philippineCulture = new CultureInfo("fil-PH");


                string formattedBaseSalary = baseSalary.ToString("C", philippineCulture);
                string formattedBonus = bonus.ToString("C", philippineCulture);
                string formattedAdvance = advance.ToString("C", philippineCulture);
                string formattedTotalSalary = totalSalary.ToString("C", philippineCulture);
                string formattedBalance = balance.ToString("C", philippineCulture);


                string receipt = $"********** PAYROLL RECEIPT **********\n";
                receipt += $"Employee ID: {employeeId}\n";
                receipt += $"Employee Name: {employeeName}\n\n";
                receipt += $"Base Salary: {formattedBaseSalary}\n";
                receipt += $"Bonus: {formattedBonus}\n";
                receipt += $"Advance: {formattedAdvance}\n";
                receipt += $"----------------------------------\n";
                receipt += $"Total Salary: {formattedTotalSalary}\n";
                receipt += $"Balance: {formattedBalance}\n";
                receipt += $"Pay Period: {payPeriod}\n";
                receipt += $"----------------------------------\n";
                receipt += $"********** END OF RECEIPT **********";


                MessageBox.Show(receipt, "Payroll Receipt");


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating receipt: " + ex.Message);
            }
        }


        private void GenerateReceiptBtn_Click(object sender, EventArgs e)
        {
            GeneratePayrollReceipt();  
        }

















        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Payroll_Load(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }


        private void DtrCB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BonusCB_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetBAmount();
        }

        private void DtrCB_SelectionChangeCommitted_1(object sender, EventArgs e)
        {
            GetDtrData();
        }

    }
}
