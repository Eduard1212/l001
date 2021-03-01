using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace WSMutovin
{
    public partial class Form1 : Form
    {
        public string connect = @"Data Source=DESKTOP-95TFA7J\SQLEXPRESS;Initial Catalog=WSDataBase;Integrated Security=True";
        public int ClientID;

        public Form1()
        {
            InitializeComponent();
        }

        //load from
        private void Form1_Load(object sender, EventArgs e)
        {
            this.dataGrid.DefaultCellStyle.ForeColor = Color.FromArgb(62, 120, 138);
            this.dataGrid.DefaultCellStyle.BackColor = Color.FromArgb(41, 44, 51);
            this.dataGrid.DefaultCellStyle.SelectionForeColor = Color.FromArgb(41, 44, 51);
            this.dataGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(62, 120, 138);

            getRefresh();
        }

        //button to exit
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //enter the client information
        public void getRefresh()
        {
            try
            {
                SqlConnection Connection = new SqlConnection(connect);

                SqlCommand command = new SqlCommand("SELECT * FROM Clients ORDER BY id", Connection);
                DataTable data = new DataTable();

                Connection.Open();

                SqlDataReader sdr = command.ExecuteReader();
                data.Load(sdr);

                Connection.Close();

                dataGrid.DataSource = data;
            }
            catch(Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //the output of the selected row in text boxes
        private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ClientID = Convert.ToInt32(dataGrid.SelectedRows[0].Cells[0].Value);

            txtFirstName.Text = dataGrid.SelectedRows[0].Cells[1].Value.ToString();
            txtMiddleName.Text = dataGrid.SelectedRows[0].Cells[2].Value.ToString();
            txtLastName.Text = dataGrid.SelectedRows[0].Cells[3].Value.ToString();
            txtPhone.Text = dataGrid.SelectedRows[0].Cells[4].Value.ToString();
            txtEmail.Text = dataGrid.SelectedRows[0].Cells[5].Value.ToString();
        }

        //create command
        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPhone.Text != "" || txtEmail.Text != "")
                {
                    SqlConnection Connection = new SqlConnection(connect);
                    string Query = "INSERT INTO Clients(firstname, middlename, lastname, phone, email) VALUES " +
                        "(" +
                        "'" + txtFirstName.Text + "'," +
                        "'" + txtMiddleName.Text + "'," +
                        "'" + txtLastName.Text + "'," +
                        "'" + txtPhone.Text + "'," +
                        "'" + txtEmail.Text + "'" +
                        ")";

                    Connection.Open();

                    SqlCommand command = new SqlCommand(Query, Connection);
                    command.ExecuteNonQuery();

                    Connection.Close();

                    getRefresh();
                    Clears();
                }
                else
                {
                    MessageBox.Show("Введите номер телефона либо адрес электронной почты!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //update command
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (ClientID > 0)
                {
                    SqlConnection Connection = new SqlConnection(connect);
                    string Query = "UPDATE Clients SET " +
                        "firstname = '" + txtFirstName.Text + "'," +
                        " middlename = '" + txtMiddleName.Text + "'," +
                        " lastname = '" + txtLastName.Text + "'," +
                        " phone = '" + txtPhone.Text + "'," +
                        " email = '" + txtEmail.Text + "'" +
                        " WHERE " +
                        "id = '" + this.ClientID + "'";
                    Connection.Open();

                    SqlCommand command = new SqlCommand(Query, Connection);
                    command.ExecuteNonQuery();

                    Connection.Close();

                    getRefresh();
                    Clears();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //delete command
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (ClientID > 0)
                {
                    if (MessageBox.Show("Вы действительно хотите удалить данного клиента?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlConnection Connection = new SqlConnection(connect);
                        string Query = "DELETE Clients WHERE id = '" + this.ClientID + "'";

                        Connection.Open();

                        SqlCommand command = new SqlCommand(Query, Connection);
                        command.ExecuteNonQuery();

                        Connection.Close();

                        getRefresh();
                        Clears();
                    }
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите клиента!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //clear command
        private void Clear_Click(object sender, EventArgs e)
        {
            Clears();
        }

        //method that clears text fields
        private void Clears()
        {
            txtEmail.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtMiddleName.Clear();
            txtPhone.Clear();

            ClientID = 0;
            try
            {
                string connect = @"Data Source=DESKTOP-CSSH9GB\SQLEXPRESS;Initial Catalog=MVC;Integrated Security=True";
                SqlConnection con = new SqlConnection(connect);
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT Name FROM [Users]";
                string fio = Convert.ToString(cmd.ExecuteScalar());
                this.label1.Text += fio;
                cmd.CommandText = "SELECT Surname FROM [Users]";
                fio = Convert.ToString(cmd.ExecuteScalar());
                this.label1.Text += fio;
                cmd.CommandText = "SELECT Patronymic FROM [Users]";
                fio = Convert.ToString(cmd.ExecuteScalar());
                this.label1.Text += fio;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Convert.ToString(ex), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
