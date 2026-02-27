using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Book_Shop_Management_System
{
    public partial class Admin_Login : Form
    {
        public Admin_Login()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkToUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            User_Login obj_user = new User_Login();
            obj_user.Show();
            this.Hide();
        }

        private void btnAdminLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill all blank fields!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtUsername.Text == "Admin" && txtPassword.Text == "Admin123")
            {
                MessageBox.Show("Log in successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Book book = new Book();
                book.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Incorrect username or password!", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void admin_showPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = admin_showPass.Checked ? '\0' : '*';
        }
    }
}
