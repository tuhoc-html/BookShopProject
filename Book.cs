using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Book_Shop_Management_System
{
    public partial class Book : Form
    {
        public Book()
        {
            InitializeComponent();
        }

        SqlConnection connect = new SqlConnection(@"Data Source=.\SQL2025;Initial Catalog=BookDB;Integrated Security=True;TrustServerCertificate=True");

        private void btnUsers_Click(object sender, EventArgs e)
        {
            User obj_user = new User();
            obj_user.Show();
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

                DGVBooks.DataSource = dt;

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
            txtAuthorName.Clear();
            txtQuantity.Clear();
            txtPrice.Clear();
            CbSelectCategory.SelectedIndex = -1;
            key = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBookTitle.Text) ||
            string.IsNullOrWhiteSpace(txtAuthorName.Text) ||
            CbSelectCategory.SelectedIndex == -1 ||
            string.IsNullOrWhiteSpace(txtQuantity.Text) ||
            string.IsNullOrWhiteSpace(txtPrice.Text))
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
                string query = "INSERT INTO BookTbl (Title, Author, Category, Quantity, Price) " +
                    "VALUES (@Title, @Author, @Category, @Quantity, @Price)";

                using (SqlCommand insertCmd = new SqlCommand(query, connect))
                {
                    insertCmd.Parameters.AddWithValue("@Title", txtBookTitle.Text);
                    insertCmd.Parameters.AddWithValue("@Author", txtAuthorName.Text);
                    insertCmd.Parameters.AddWithValue("@Category", CbSelectCategory.SelectedItem.ToString());
                    insertCmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                    insertCmd.Parameters.AddWithValue("@Price", txtPrice.Text);

                    insertCmd.ExecuteNonQuery();

                    MessageBox.Show("Book Added Successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DisplayBooks();
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

        private void Book_Load(object sender, EventArgs e)
        {
            DisplayBooks();
        }

        int key = 0;
        private void DGVBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DGVBooks.Rows[e.RowIndex];

                txtBookTitle.Text = row.Cells[1].Value?.ToString() ?? "";
                txtAuthorName.Text = row.Cells[2].Value?.ToString() ?? "";
                CbSelectCategory.SelectedItem = row.Cells[3].Value?.ToString();
                txtQuantity.Text = row.Cells[4].Value?.ToString() ?? "";
                txtPrice.Text = row.Cells[5].Value?.ToString() ?? "";

                if (string.IsNullOrEmpty(txtBookTitle.Text))
                {
                    key = 0;
                }
                else
                {
                    key = Convert.ToInt32(row.Cells[0].Value);
                }
            }

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Please select a book to edit!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtBookTitle.Text) ||
            string.IsNullOrWhiteSpace(txtAuthorName.Text) ||
            CbSelectCategory.SelectedIndex == -1 ||
            string.IsNullOrWhiteSpace(txtQuantity.Text) ||
            string.IsNullOrWhiteSpace(txtPrice.Text))
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
                string query = "UPDATE BookTbl SET Title=@Title, Author=@Author, Category=@Category, Quantity=@Quantity, Price=@Price WHERE BookId=@Key";
                using (SqlCommand updateCmd = new SqlCommand(query, connect))
                {
                    updateCmd.Parameters.AddWithValue("@Title", txtBookTitle.Text);
                    updateCmd.Parameters.AddWithValue("@Author", txtAuthorName.Text);
                    updateCmd.Parameters.AddWithValue("@Category", CbSelectCategory.SelectedItem.ToString());
                    updateCmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text);
                    updateCmd.Parameters.AddWithValue("@Price", txtPrice.Text);
                    updateCmd.Parameters.AddWithValue("@Key", key);
                    updateCmd.ExecuteNonQuery();
                    MessageBox.Show("Book Updated Successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisplayBooks();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (key == 0)
            {
                MessageBox.Show("Please select a book to delete!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this book?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    if (connect.State == ConnectionState.Closed)
                    {
                        connect.Open();
                    }
                    string query = "DELETE FROM BookTbl WHERE BookId=@Key";

                    using (SqlCommand deleteCmd = new SqlCommand(query, connect))
                    {
                        deleteCmd.Parameters.AddWithValue("@Key", key);
                        deleteCmd.ExecuteNonQuery();
                        MessageBox.Show("Book Deleted Successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisplayBooks();
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            CbFilterByCategory.SelectedIndex = -1;
            DisplayBooks();
        }

        private void CbFilterByCategory_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (CbFilterByCategory.SelectedIndex == -1)
            {
                DisplayBooks();
                return;
            }

            try
            {
                if (connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                string query = "SELECT * FROM BookTbl WHERE Category=@Category";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                adapter.SelectCommand.Parameters.AddWithValue("@Category", CbFilterByCategory.SelectedItem.ToString());
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                DGVBooks.DataSource = dt;
                connect.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (connect.State == ConnectionState.Open) connect.Close();
            }
        }
    }
}
