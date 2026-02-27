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
    public partial class Billing : Form
    {
        public Billing()
        {
            InitializeComponent();
        }

        SqlConnection connect = new SqlConnection(@"Data Source=.\SQL2025;Initial Catalog=BookDB;Integrated Security=True;TrustServerCertificate=True");

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

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Admin_Login obj_admin = new Admin_Login();
            obj_admin.Show();
            this.Hide();
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            Admin_Login obj_admin = new Admin_Login();
            obj_admin.Show();
            this.Hide();
        }

        private void btnBooks_Click(object sender, EventArgs e)
        {
            Admin_Login obj_admin = new Admin_Login();
            obj_admin.Show();
            this.Hide();
        }

        private void DisplayBooks()
        {
            try
            {
                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }

                string query = "SELECT * FROM BookTbl";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                DGVBook.DataSource = dt;

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
            txtBookTitle.Clear();
            txtCustomerName.Clear();
            txtQuantity.Clear();
            txtPrice.Clear();
        }

        private void Billing_Load(object sender, EventArgs e)
        {
            DisplayBooks();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        int key, stock = 0;
        private void DGVBook_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DGVBook.Rows[e.RowIndex];

                txtBookTitle.Text = row.Cells[1].Value?.ToString();
                txtPrice.Text = row.Cells[5].Value?.ToString();

                if (string.IsNullOrEmpty(txtBookTitle.Text))
                {
                    key = 0;
                    stock = 0;
                }
                else
                {
                    key = Convert.ToInt32(row.Cells[0].Value);
                    stock = Convert.ToInt32(row.Cells[4].Value);
                }
            }
        }
    }
}
