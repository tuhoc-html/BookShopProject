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
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void btnBooks_Click(object sender, EventArgs e)
        {
            Book obj_book = new Book();
            obj_book.Show();
            this.Hide();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            User obj_user = new User();
            obj_user.Show();
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
    }
}
