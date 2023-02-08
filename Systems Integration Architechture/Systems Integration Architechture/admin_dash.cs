using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace Systems_Integration_Architechture
{
    public partial class admin_dash : Form
    {
        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; lblName.Text = _username; }
        }

        MySqlConnection connection = DBClass.connection;
        public admin_dash()
        {
            InitializeComponent();
            
        }

        private void bunifuLabel2_Click(object sender, EventArgs e)
        {
            panel1.BackColor = Color.White;
            panel2.BackColor = Color.FromArgb(156, 29, 179);
            panel3.BackColor = Color.White;
            panel9.BackColor = Color.White;

            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = Properties.Resources.dashboard_black;

            pictureBox3.Image = Properties.Resources.userCreation_white1;
            pictureBox7.Image = Properties.Resources.user_black;
            pictureBox4.Image = Properties.Resources.event_black;

            pages.SelectedIndex = 1;

        }

        private void bunifuLabel3_Click(object sender, EventArgs e)
        {
            panel1.BackColor = Color.White;
            panel2.BackColor = Color.White;
            panel3.BackColor = Color.FromArgb(156, 29, 179);
            panel9.BackColor = Color.White;

            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = Properties.Resources.dashboard_black;

            pictureBox3.Image = Properties.Resources.userCreation_white;
            pictureBox7.Image = Properties.Resources.user_black;
            pictureBox4.Image = Properties.Resources.event_white;

            pages.SelectedIndex = 2;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuLabel1_Click(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(156, 29, 179);
            panel2.BackColor = Color.White;
            panel3.BackColor = Color.White;
            panel9.BackColor = Color.White;


            pictureBox2.BackColor = Color.FromArgb(156, 29, 179);
            pictureBox2.Image = Properties.Resources.dashboard_layout_144px;

            pictureBox3.Image = Properties.Resources.userCreation_white;
            pictureBox7.Image = Properties.Resources.user_black;
            pictureBox4.Image = Properties.Resources.event_black;

            pages.SelectedIndex = 0;

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }


        //PROGRAM LOGICS
        private void admin_dash_Load(object sender, EventArgs e)
        {

            LoadTotalEvents();

            populateUserTypeDropDown();
            //Initial load of data into DataGridView
            LoadData();

            //Initial load of total number of users into label
            LoadTotalUsers();

            LoadEvents();
            ////Create a new Timer object
            //Timer refreshTimer = new Timer();

            ////Set the interval for the timer (in milliseconds)
            //refreshTimer.Interval = 100;

            ////Subscribe to the Tick event of the timer
            //refreshTimer.Tick += new EventHandler(RefreshData);

            ////Start the timer
            //refreshTimer.Start();
        }

        private void LoadData()
        {
            connection.Close();
            try
            {
                connection.Open();
                string query = "SELECT `id`, `fname`, `lname`, `username`, `email` FROM `user_info`";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                DataTable dt = new DataTable();
                MySqlDataReader read_ = cmd.ExecuteReader();
                dt.Load(read_);
                bunifuDataGridView2.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
            finally
            {
                connection.Close();
            }
        }

        private void LoadTotalUsers()
        {
            try
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM `user_info`";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                int totalUsers = Convert.ToInt32(cmd.ExecuteScalar());
                lblUserCount.Text = totalUsers.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
            finally
            {
                connection.Close();
            }
        }

        private void LoadEvents()
        {
            try
            {
                connection.Open();
                string query = "SELECT `id`, `event_name`, `location`, `event_day` FROM `events` WHERE 1";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                DataTable dt = new DataTable();
                MySqlDataReader read_ = cmd.ExecuteReader();
                dt.Load(read_);
                bunifuDataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
            finally
            {
                connection.Close();
            }
        }

        //private void RefreshData(object sender, EventArgs e)
        //{ 

        //    //Reload total number of users into label
        //    LoadTotalUsers();
        //}

        private void populateUserTypeDropDown()
        {
            bunifuDropdown1.Items.Add("admin");
            bunifuDropdown1.Items.Add("user");
        }
        private void bunifuLabel4_Click(object sender, EventArgs e)
        {
            panel1.BackColor = Color.White;
            panel2.BackColor = Color.White;
            panel3.BackColor = Color.White;
            panel9.BackColor = Color.FromArgb(156, 29, 179);

            pictureBox7.BackColor = Color.Transparent;
            pictureBox7.Image = Properties.Resources.users_white;

            pictureBox3.Image = Properties.Resources.userCreation_white;
            pictureBox4.Image = Properties.Resources.event_black;

            pages.SelectedIndex = 3;
        }

        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            //Reload data into DataGridView
            LoadData();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            connection.Close();
            try
            {
                if(connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    if (!this.txtEmail.Text.Contains('@') || !this.txtEmail.Text.Contains('.'))
                    {
                        MessageBox.Show("Please Enter A Valid Email", "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtEmail.Text))
                    {
                        MessageBox.Show("Please fill out all information!", "Error");
                        return;
                    }

                    else
                    {

                        MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM user_info WHERE username = @UserName", connection),
                        cmd2 = new MySqlCommand("SELECT * FROM user_info WHERE email = @UserEmail", connection);


                        cmd1.Parameters.AddWithValue("@UserName", txtUsername.Text);
                        cmd2.Parameters.AddWithValue("@UserEmail", txtEmail.Text);

                        bool userExists = false, mailExists = false;

                        using (var dr1 = cmd1.ExecuteReader())
                        {
                            if (userExists = dr1.HasRows) MessageBox.Show("Username not available!");
                            dr1.Close();
                        }

                        using (var dr2 = cmd2.ExecuteReader())
                        {
                            if (mailExists = dr2.HasRows) MessageBox.Show("Email not available!");
                            dr2.Close();
                        }

                        if (!(userExists || mailExists))
                        {
                            byte[] passwordBytes = Encoding.UTF8.GetBytes(txtPassword.Text);
                            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();
                            byte[] passwordHash = sha256.ComputeHash(passwordBytes);
                            string passwordHashString = BitConverter.ToString(passwordHash).Replace("-", "");

                            DateTime dateCreated = DateTime.Now;
                            string iquery = "INSERT INTO `user_info`(`id`, `fname`, `lname`, `username`, `password`, `email`, `user_type`, `date_created`, `is_online`) " +
                                "VALUES (id, @fname, @lname , @username , @password , @email , @user_type , @date_created , 0)";
                            MySqlCommand commandDatabase = new MySqlCommand(iquery, connection);
                            commandDatabase.CommandTimeout = 60;
                            commandDatabase.Parameters.AddWithValue("@fname", txtFname.Text);
                            commandDatabase.Parameters.AddWithValue("@lname", txtLname.Text);
                            commandDatabase.Parameters.AddWithValue("@username", txtUsername.Text);
                            commandDatabase.Parameters.AddWithValue("@password", passwordHashString);
                            commandDatabase.Parameters.AddWithValue("@email", txtEmail.Text);
                            commandDatabase.Parameters.AddWithValue("@date_created", dateCreated);
                            commandDatabase.Parameters.AddWithValue("@user_type", bunifuDropdown1.SelectedItem);
                            try
                            {
                                commandDatabase.ExecuteNonQuery();
                                MessageBox.Show("Account Created!");
                            }
                            catch (Exception ex)
                            {
                                // Show any error message.
                                MessageBox.Show(ex.Message);
                            }
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

        private void LoadTotalEvents()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string query = "SELECT COUNT(*) FROM `events`";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                int totalUsers = Convert.ToInt32(cmd.ExecuteScalar());
                lblEventStat.Text = totalUsers.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
            finally
            {
                connection.Close();
            }
        }

        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            LoadTotalUsers();
            LoadTotalEvents();
        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            admin_login ad = new admin_login();
            Hide();
            ad.ShowDialog();
            Close();
        }
    }
}
