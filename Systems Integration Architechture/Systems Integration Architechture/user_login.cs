using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace Systems_Integration_Architechture
{
    public partial class user_login : Form
    {
        MySqlConnection connection = DBClass.connection;
        //string storedUsername = "";
        public user_login()
        {
            InitializeComponent();

        }

        private void admin_login_Load(object sender, EventArgs e)
        {

        }


        private void bunifuCustomTextbox1_Click(object sender, EventArgs e)
        {
            txtUsername.BorderColor = Color.FromArgb(27, 116, 237);
            lblUsername.ForeColor = Color.FromArgb(27, 116, 237);
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Please input Username and Password", "Error");
            }
            else
            {
                connection.Open();
                //PASSWORD ENCRYPTION

                string selectQuery = "SELECT * FROM user_info WHERE username = @username AND user_type = @user_role;";
                MySqlCommand command = new MySqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@username", txtUsername.Text);
                command.Parameters.AddWithValue("@user_role", "user");
                MySqlDataReader mdr = command.ExecuteReader();
                if (mdr.Read())
                {
                    mdr.Close();  // close the datareader here
                                  //QUERIES
                    
                    animate.ShowSync(bunifuPages1);
                    bunifuPages1.SelectedIndex = 1;

                }
                else
                {
                    lblerror.Visible = true;
                }

                connection.Close();
            }

        }

       

        private void bunifuButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if(connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    if (string.IsNullOrEmpty(txtPass.Text))
                    {
                        MessageBox.Show("Please input your Password", "Error");
                    }
                    else
                    {
                        //PASSWORD ENCRYPTION
                        byte[] passwordBytes = Encoding.UTF8.GetBytes(txtPass.Text);
                        SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                        byte[] passwordHash = sha256.ComputeHash(passwordBytes);
                        string passwordHashString = BitConverter.ToString(passwordHash).Replace("-", "");               

                        string selectQuery = "SELECT * FROM user_info WHERE username = @username AND password = @password AND user_type = @usertype;";
                        MySqlCommand command = new MySqlCommand(selectQuery, connection);
                        command.Parameters.AddWithValue("@username", txtUsername.Text);
                        command.Parameters.AddWithValue("@password", passwordHashString);
                        command.Parameters.AddWithValue("@usertype", "user");
                        MySqlDataReader mdr = command.ExecuteReader();
                        if (mdr.Read())
                        {
                            mdr.Close();  // close the datareader here
                                          //QUERIES
                            user_dash dash = new user_dash();
                            Hide();
                            dash.Username = txtUsername.Text;
                            dash.ShowDialog();
                            Close();

                        }
                        else
                        {
                            lblpasserror.Visible = true;
                        }

                        connection.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuButton2_Click(object sender, EventArgs e)
        {
            selection sel = new selection();
            Hide();
            sel.ShowDialog();
            Close();
        }

        private void bunifuButton4_Click(object sender, EventArgs e)
        {
            bunifuPages1.SelectedIndex = 0;
        }
    }
}
