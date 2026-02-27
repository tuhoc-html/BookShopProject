using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Book_Shop_Management_System
{
    public partial class User : Form
    {
        public User()
        {
            InitializeComponent();
        }

        SqlConnection connect = new SqlConnection(@"Data Source=.\SQL2025;Initial Catalog=BookDB;Integrated Security=True;TrustServerCertificate=True");

        private void DisplayUsers()
        {
            try
            {
                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                string query = "SELECT * FROM UserTbl";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                DGVUser.DataSource = dt;

                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                if (connect.State == ConnectionState.Open) connect.Close();
            }
        }

        private void Reset()
        {
            txtUsername.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            txtPassword.Clear();
        }

        private void btnBooks_Click(object sender, EventArgs e)
        {
            Book obj_book = new Book();
            obj_book.Show();
            this.Hide();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard obj_dashboard = new Dashboard();
            obj_dashboard.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            User_Login obj_user = new User_Login();
            obj_user.Show();
            this.Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
            string.IsNullOrWhiteSpace(txtPhone.Text) ||
            string.IsNullOrWhiteSpace(txtAddress.Text) ||
            string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill in all fields!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                string query = "INSERT INTO UserTbl (Username, UserPhone, UserAddress, UserPassword) " +
                    "VALUES (@Username, @UserPhone, @UserAddress, @UserPassword)";

                using (SqlCommand insertCmd = new SqlCommand(query, connect))
                {
                    insertCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                    insertCmd.Parameters.AddWithValue("@UserPhone", txtPhone.Text);
                    insertCmd.Parameters.AddWithValue("@UserAddress", txtAddress.Text);
                    insertCmd.Parameters.AddWithValue("@UserPassword", txtPassword.Text);

                    insertCmd.ExecuteNonQuery();

                    MessageBox.Show("User Added Successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DisplayUsers();
                    Reset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connect.State == ConnectionState.Open) connect.Close();
            }
        }

        private void User_Load(object sender, EventArgs e)
        {
            DisplayUsers();
        }

        int key = 0;
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Please select a user to edit!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
            string.IsNullOrWhiteSpace(txtPhone.Text) ||
            string.IsNullOrWhiteSpace(txtAddress.Text) ||
            string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill in all the information before updating.", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                string query = "UPDATE UserTbl SET Username=@Username, UserPhone=@UserPhone, UserAddress=@UserAddress, UserPassword=@UserPassword WHERE UserId=@Key";
                using (SqlCommand updateCmd = new SqlCommand(query, connect))
                {
                    updateCmd.Parameters.AddWithValue("@Username", txtUsername.Text);
                    updateCmd.Parameters.AddWithValue("@UserPhone", txtPhone.Text);
                    updateCmd.Parameters.AddWithValue("@UserAddress", txtAddress.Text);
                    updateCmd.Parameters.AddWithValue("@UserPassword", txtPassword.Text);
                    updateCmd.Parameters.AddWithValue("@Key", key);
                    updateCmd.ExecuteNonQuery();
                    MessageBox.Show("User Updated Successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisplayUsers();
                    Reset();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (connect.State == ConnectionState.Open) connect.Close();
            }
        }

        private void DGVUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DGVUser.Rows[e.RowIndex];

                txtUsername.Text = row.Cells[1].Value?.ToString() ?? "";
                txtPhone.Text = row.Cells[2].Value?.ToString() ?? "";
                txtAddress.Text = row.Cells[3].Value?.ToString() ?? "";
                txtPassword.Text = row.Cells[4].Value?.ToString() ?? "";

                if (string.IsNullOrEmpty(txtUsername.Text))
                {
                    key = 0;
                }
                else
                {
                    key = Convert.ToInt32(row.Cells[0].Value);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Please select a user to delete!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this user?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }
                    string query = "DELETE FROM UserTbl WHERE UserId=@Key";

                    using (SqlCommand deleteCmd = new SqlCommand(query, connect))
                    {
                        deleteCmd.Parameters.AddWithValue("@Key", key);
                        deleteCmd.ExecuteNonQuery();
                        MessageBox.Show("User Deleted Successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayUsers();
                        Reset();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (connect.State == ConnectionState.Open) connect.Close();
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }
    }
}
