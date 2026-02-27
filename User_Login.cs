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

namespace Book_Shop_Management_System
{
    public partial class User_Login : Form
    {
        public User_Login()
        {
            InitializeComponent();
        }

        SqlConnection connect = new SqlConnection(@"Data Source=.\SQL2025;Initial Catalog=BookDB;Integrated Security=True;TrustServerCertificate=True");

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkToAdmin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Admin_Login obj_admin = new Admin_Login();
            obj_admin.Show();
            this.Hide();
        }

        private void btnUserLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill all blank fields!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (connect.State == ConnectionState.Closed) connect.Open();

                string query = "SELECT COUNT(*) FROM UserTbl WHERE Username=@Username AND UserPassword=@Password";

                using (SqlCommand cmd = new SqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                    int userCount = (int)cmd.ExecuteScalar();
                    if (userCount > 0)
                    {
                        MessageBox.Show("Log in successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Billing obj_bill = new Billing();
                        obj_bill.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect username or password!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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

        private void user_showPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = user_showPass.Checked ? '\0' : '*';
        }
    }
}
