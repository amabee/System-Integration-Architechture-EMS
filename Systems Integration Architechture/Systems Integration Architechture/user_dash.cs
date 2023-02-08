using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec;
using MySql.Data.MySqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using DGVPrinterHelper;


namespace Systems_Integration_Architechture
{
    public partial class user_dash : Form
    {
        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; lblName.Text = _username; }
        }

        //For Camera
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;

        MySqlConnection connection = DBClass.connection;
        public user_dash()
        {
            InitializeComponent();
            
        }

        private void bunifuLabel2_Click(object sender, EventArgs e)
        {
            panel1.BackColor = Color.White;
            panel2.BackColor = Color.FromArgb(156, 29, 179);
            panel3.BackColor = Color.White;

            bunifuLabel1.ForeColor = Color.Black;
            bunifuLabel2.ForeColor = Color.White;
            bunifuLabel3.ForeColor = Color.Black;

            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = Properties.Resources.dashboard_black;
            pictureBox3.Image = Properties.Resources.event_white;
            pictureBox4.Image = Properties.Resources.event_black;

            panel9.BackColor = Color.White;
            bunifuLabel7.ForeColor = Color.Black;
            pictureBox7.Image = Properties.Resources.event_black;

            pages.SelectedIndex = 1;

        }

        private void bunifuLabel3_Click(object sender, EventArgs e)
        {
            panel1.BackColor = Color.White;
            panel2.BackColor = Color.White;
            panel3.BackColor = Color.FromArgb(156, 29, 179);

            panel9.BackColor = Color.White;
            bunifuLabel7.ForeColor = Color.Black;
            pictureBox7.Image = Properties.Resources.event_black;


            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = Properties.Resources.dashboard_black;
            pictureBox3.Image = Properties.Resources.event_black;
            pictureBox4.Image = Properties.Resources.event_white;

            bunifuLabel1.ForeColor = Color.Black;
            bunifuLabel2.ForeColor = Color.Black;
            bunifuLabel3.ForeColor = Color.White;

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
            bunifuLabel7.ForeColor = Color.Black;
            pictureBox7.Image = Properties.Resources.event_black;

            bunifuLabel1.ForeColor = Color.White;
            bunifuLabel2.ForeColor = Color.Black;
            bunifuLabel3.ForeColor = Color.Black;
            


            pictureBox2.BackColor = Color.FromArgb(156, 29, 179);
            pictureBox2.Image = Properties.Resources.dashboard_layout_144px;
            pictureBox3.Image = Properties.Resources.event_black;
            pictureBox4.Image = Properties.Resources.event_black;

            pages.SelectedIndex = 0;

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

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

       

        //PROGRAM LOGICS
        private void admin_dash_Load(object sender, EventArgs e)
        {
            LoadTotalEvents();

            LoadData();

            populateEventsinAttendance();

            LoadTotalUsers();

            LoadEvents();
           

            populateEventStartUpdate();

            GetAbsentCount();

            stats_timer.Start();

        }

        private void LoadData()
        {
            connection.Close();
            try
            {
                connection.Open();
                string query = "SELECT `unique_id`, `student_id`, `fname`, `lname`, `course`, `year`, `event_attending` FROM `student_info`";
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
                string query = "SELECT COUNT(*) FROM `user_info` WHERE user_type = 'user'";
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
                string query = "SELECT `id`, `event_name`, `location`, `event_day`,`event_status` FROM `events`";
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

 

        //GET ABSENT
        private int GetAbsentCount()
        {
            int absentcounts = 0;
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string query = "SELECT COUNT(*) FROM `attendance` WHERE `event_name` = @eventname AND `isPresent` = @isPresent?";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@isPresent?", "Absent");
                cmd.Parameters.AddWithValue("@eventname", drpEventsAtt.SelectedItem);
                int countInt = Convert.ToInt32(cmd.ExecuteScalar());
                absentcounts = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return absentcounts;

        }

        private void populateEventsinAttendance()
        {

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string query = "SELECT `event_name`, `event_status` FROM `events`";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader _read = cmd.ExecuteReader();

                while (_read.Read())
                {
                    drpEventsAtt.Items.Add(_read.GetString("event_name"));
                }
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

        private void populateEventStartUpdate()
        {

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string query = "SELECT `event_name`, `event_status` FROM `events` WHERE event_status = 'Ongoing'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader _read = cmd.ExecuteReader();

                while (_read.Read())
                {
                    eventDrop.Items.Add(_read.GetString("event_name"));
                }
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

        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            //Reload data into DataGridView
            LoadData();
        }

        //private void bunifuFlatButton1_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtEventname.Text) || string.IsNullOrEmpty(txtEventOrganizer.Text) || string.IsNullOrEmpty(txtEventLocation.Text))
        //    {
        //        MessageBox.Show("Please Don't leave any blank space", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    else
        //    {
        //        if (connection.State != ConnectionState.Open)
        //        {
        //            connection.Open();
        //            string dateStart = bunifuDatepicker1.Value.ToString("yyyy-MM-dd");
        //            string dateEnd = date2.Value.ToString("yyyy-MM-dd");
        //            string iquery = "INSERT INTO `events`(`id`, `event_name`, `location`, `event_day`, `event_end`, `event_organizer`, `event_status`)" +
        //                " VALUES (id, @eventname, @eventloc, @event_date , @event_end, @eventorg , @status)";
        //            MySqlCommand commandDatabase = new MySqlCommand(iquery, connection);
        //            commandDatabase.CommandTimeout = 60;
        //            commandDatabase.Parameters.AddWithValue("@eventname", txtEventname.Text);
        //            commandDatabase.Parameters.AddWithValue("@eventorg", txtEventOrganizer.Text);
        //            commandDatabase.Parameters.AddWithValue("@eventloc", txtEventLocation.Text);
        //            commandDatabase.Parameters.AddWithValue("@event_date", dateStart);
        //            commandDatabase.Parameters.AddWithValue("@event_end", dateEnd);
        //            commandDatabase.Parameters.AddWithValue("@status", "Ongoing");

        //            commandDatabase.Parameters.AddWithValue("@isComplete?", "Upcoming");

        //            try
        //            {
        //                //MySqlDataReader myReader = commandDatabase.ExecuteReader();

        //                DialogResult _res_ = MessageBox.Show("Event Successfully Created!", "Success", MessageBoxButtons.OK);

        //                if (_res_ == DialogResult.OK)
        //                {
        //                    txtEventLocation.Text = "";
        //                    txtEventname.Text = "";
        //                    txtEventOrganizer.Text = "";
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                // Show any error message.
        //                MessageBox.Show(ex.Message);
        //            }

        //            finally
        //            {

        //                connection.Close();
        //            }

        //        }

        //    }

        //}

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEventname.Text) || string.IsNullOrEmpty(txtEventOrganizer.Text) || string.IsNullOrEmpty(txtEventLocation.Text))
            {
                MessageBox.Show("Please Don't leave any blank space", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    string dateStart = bunifuDatepicker1.Value.ToString("yyyy-MM-dd");
                    string dateEnd = date2.Value.ToString("yyyy-MM-dd");
                    string iquery = "INSERT INTO `events`(`id`, `event_name`, `location`, `event_day`, `event_end`, `event_organizer`, `event_status`)" +
                        " VALUES (id, @eventname, @eventloc, @event_date , @event_end, @eventorg , @status)";
                    MySqlCommand commandDatabase = new MySqlCommand(iquery, connection);
                    commandDatabase.CommandTimeout = 60;
                    commandDatabase.Parameters.AddWithValue("@eventname", txtEventname.Text);
                    commandDatabase.Parameters.AddWithValue("@eventorg", txtEventOrganizer.Text);
                    commandDatabase.Parameters.AddWithValue("@eventloc", txtEventLocation.Text);
                    commandDatabase.Parameters.AddWithValue("@event_date", dateStart);
                    commandDatabase.Parameters.AddWithValue("@event_end", dateEnd);
                    commandDatabase.Parameters.AddWithValue("@status", "Ongoing");

                    int rowsAffected = commandDatabase.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        DialogResult _res_ = MessageBox.Show("Event Successfully Created!", "Success", MessageBoxButtons.OK);

                        if (_res_ == DialogResult.OK)
                        {
                            txtEventLocation.Text = "";
                            txtEventname.Text = "";
                            txtEventOrganizer.Text = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("The event could not be created", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    connection.Close();
                }
            }
        }



        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            LoadTotalUsers(); 
            LoadTotalEvents();
        }

        private void bunifuLabel7_Click(object sender, EventArgs e)
        {
            panel9.BackColor = Color.FromArgb(156, 29, 179);
            bunifuLabel7.ForeColor = Color.White;
            pictureBox7.Image = Properties.Resources.event_white;

            panel1.BackColor = Color.White;
            panel2.BackColor = Color.White;
            panel3.BackColor = Color.White;

            bunifuLabel1.ForeColor = Color.Black;
            bunifuLabel2.ForeColor = Color.Black;
            bunifuLabel3.ForeColor = Color.Black;



            pictureBox2.BackColor = Color.FromArgb(156, 29, 179);
            pictureBox2.Image = Properties.Resources.dashboard_black;
            pictureBox3.Image = Properties.Resources.event_black;
            pictureBox4.Image = Properties.Resources.event_black;

            pages.SelectedIndex = 3;


        }

        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            pages.SelectedIndex = 0;
        }

        private void bunifuImageButton6_Click(object sender, EventArgs e)
        {
            LoadEvents();
            populateEventsinAttendance();
            populateEventStartUpdate();
            LoadTotalUsers();
        }

        private void drpEventsAtt_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string query = "SELECT * FROM `attendance` WHERE `event_name` = @event_status";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@event_status", drpEventsAtt.SelectedItem);
                DataTable dt = new DataTable();
                MySqlDataReader read_ = cmd.ExecuteReader();
                dt.Load(read_);
                bunifuDataGridView2.DataSource = dt;
                bunifuDataGridView2.Columns[0].HeaderText = "ID";
                bunifuDataGridView2.Columns[1].HeaderText = "Student ID";
                bunifuDataGridView2.Columns[2].HeaderText = "Firstname";
                bunifuDataGridView2.Columns[3].HeaderText = "Lastname";
                bunifuDataGridView2.Columns[4].HeaderText = "Course";
                bunifuDataGridView2.Columns[5].HeaderText = "Year Level";
                bunifuDataGridView2.Columns[6].HeaderText = "Event Name";
                bunifuDataGridView2.Columns[7].HeaderText = "Attendace";
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



        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            pages.SelectedIndex = 4;
            lblEventQR.Text = eventDrop.SelectedItem.ToString();

        }

        private void eventDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string query = "SELECT * FROM `attendance` WHERE `event_name` = @event_name";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@event_name", drpEventsAtt.SelectedItem);
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

        private void bunifuLabel23_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            qr_timer1.Start();
        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo Device in CaptureDevice)
                bunifuDropdown1.Items.Add(Device.Name);

            bunifuDropdown1.SelectedIndex = 0;
            FinalFrame = new VideoCaptureDevice();

            CameraOnLoad();
        }
        private void CameraOnLoad()
        {
            FinalFrame = new VideoCaptureDevice(CaptureDevice[bunifuDropdown1.SelectedIndex].MonikerString);
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            FinalFrame.Start();
        }
        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            pictureBox11.Image = (Bitmap)eventArgs.Frame.Clone();
        }

        private void StartScanning()
        {

            BarcodeReader reader = new BarcodeReader();
            Result result = reader.Decode((Bitmap)pictureBox11.Image);
            if (result != null)
            {
                try
                {
                    string decoded = result.ToString().Trim();
                    txtStudID.Text = decoded;
                    if (decoded != null)
                    {
                        if (connection.State != ConnectionState.Open)
                        {
                            connection.Open();
                            MySqlCommand coman = new MySqlCommand();
                            coman.Connection = connection;
                            coman.CommandText = "SELECT * from student_info WHERE `student_id` = @studentid";
                            coman.Parameters.AddWithValue("@studentid", txtStudID.Text);
                            MySqlDataReader dr = coman.ExecuteReader();
                            dr.Read();
                            if (dr.HasRows)
                            {
                                txtStudID.Text = dr["student_id"].ToString();
                                txtFname.Text = dr["fname"].ToString();
                                txtLname.Text = dr["lname"].ToString();
                                txtCourse.Text = dr["course"].ToString();
                                txtyrLvl.Text = dr["year"].ToString();
                            }

                            connection.Close();
                            qr_timer2.Start();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }
            }

            //// Add a delay to the loop to reduce resource usage.
            Thread.Sleep(100);
        }

        private void qr_timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                pictureBox11.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] Photo = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(Photo, 0, Photo.Length);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM student_info WHERE `student_id` = @studentid AND event_attending = @eventName";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@studentid", txtStudID.Text);
                    cmd.Parameters.AddWithValue("@eventName", lblEventQR.Text);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        string checkQuery = "SELECT COUNT(*) FROM attendance WHERE `student_id` = @studentID AND event_name = @selectedEvent";
                        MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                        checkCommand.Parameters.AddWithValue("@studentID", txtStudID.Text);
                        checkCommand.Parameters.AddWithValue("@selectedEvent", lblEventQR.Text);
                        int att_count = Convert.ToInt32(checkCommand.ExecuteScalar());
                        if (att_count > 0)
                        {
                            // The student is already present in the attendance table.
                            MessageBox.Show("The Student is already Present in the Event!");
                        }
                        else
                        {
                            string iquery = "INSERT INTO `attendance`(`id`, `student_id`, `student_fname`, `student_lname`, `student_course`, `student_yrLvl`, `event_name`, `isPresent`)" +
                                " VALUES (id, @studentID , @studentFname, @studentLname , @studentCourse , @studentYrlvl , @selectedEvent, @Present )";
                            MySqlCommand commandDatabase = new MySqlCommand(iquery, connection);
                            commandDatabase.CommandTimeout = 60;
                            commandDatabase.Parameters.AddWithValue("@studentID", txtStudID.Text);
                            commandDatabase.Parameters.AddWithValue("@studentFname", txtFname.Text);
                            commandDatabase.Parameters.AddWithValue("@studentLname", txtLname.Text);
                            commandDatabase.Parameters.AddWithValue("@studentCourse", txtCourse.Text);
                            commandDatabase.Parameters.AddWithValue("@studentYrlvl", txtyrLvl.Text);
                            commandDatabase.Parameters.AddWithValue("@selectedEvent", lblEventQR.Text);
                            commandDatabase.Parameters.AddWithValue("@Present", "Present");

                            try
                            {
                                MySqlDataReader myReader = commandDatabase.ExecuteReader();

                                DialogResult result = MessageBox.Show("Attendance Updated!", "Windows Alert", MessageBoxButtons.OK);

                                if (result == DialogResult.OK)
                                {
                                    qr_timer1.Stop();
                                    qr_timer2.Stop();
                                }
                            }
                            catch (Exception ex)
                            {
                                // Show any error message.
                                MessageBox.Show(ex.Message);
                            }

                        }
                    }
                    else
                    {
                        qr_timer1.Stop();
                        qr_timer2.Stop();

                        DialogResult notReg = MessageBox.Show("User is not pre-registered for the event", "Windows Notification", MessageBoxButtons.OK);
                        if (notReg == DialogResult.OK)
                        {
                            
                        }
                    }
                }
                connection.Close();
                qr_timer2.Enabled = false;
                qr_timer1.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }

        private void qr_timer1_Tick(object sender, EventArgs e)
        {
            StartScanning();
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string eventName = eventDrop.SelectedItem.ToString();
                string updateQuery = "UPDATE `events` SET `event_status`= @isComplete WHERE `event_name`= @event_name";
                MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                updateCmd.Parameters.AddWithValue("@isComplete", "Completed");
                updateCmd.Parameters.AddWithValue("@event_name", eventName);
                updateCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
            markAsAbsent();
        }

        private void markAsAbsent()
        {
            List<string> studentIDs = new List<string>();
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string query = "SELECT `student_id`, `event_attending` FROM `student_info` WHERE `event_attending` = @event_att ";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@event_att", eventDrop.SelectedItem);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    studentIDs.Add(reader["student_id"].ToString());
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            foreach (string studentID in studentIDs)
            {
                try
                {
                    if (connection.State != ConnectionState.Open)
                        connection.Open();
                    string query = "SELECT COUNT(*) FROM `attendance` WHERE `student_id` = @studentID AND `event_name` = @event_name ";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@studentID", studentID);
                    cmd.Parameters.AddWithValue("@event_name", eventDrop.SelectedItem);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        // insert the student as absent
                        string iquery = "INSERT INTO `attendance`(`student_id`, `student_fname`, `student_lname`, `student_course`, `student_yrLvl`, `event_name`, `isPresent`)" +
                                        " SELECT `student_id`, `fname`, `lname`, `course`, `year`, @event_name, @isPresent" +
                                        " FROM `student_info` WHERE `student_id` = @studentID";
                        MySqlCommand commandDatabase = new MySqlCommand(iquery, connection);
                        commandDatabase.Parameters.AddWithValue("@studentID", studentID);
                        commandDatabase.Parameters.AddWithValue("@event_name", eventDrop.SelectedItem);
                        commandDatabase.Parameters.AddWithValue("@isPresent", "Absent");
                        commandDatabase.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            connection.Close();
        }

        private int GetAttendedCount()
        {
            int count = 0;
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string query = "SELECT COUNT(*) FROM `attendance` WHERE `event_name` = @eventname AND `isPresent` = @isPresent?";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@eventname", drpEventsAtt.SelectedItem);
                cmd.Parameters.AddWithValue("@isPresent?", "Present");
                int countInt = Convert.ToInt32(cmd.ExecuteScalar());
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return count;
        }

        private int GetPreReg()
        {
            int count = 0;
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string query = "SELECT COUNT(*) FROM `student_info` WHERE `event_attending` = @event_attending";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@event_attending", drpEventsAtt.SelectedItem);
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return count;
        }

        private void stats_timer_Tick(object sender, EventArgs e)
        {
            int getAbsentCount = GetAbsentCount();
            AbsentsLbl.Text = getAbsentCount.ToString();

            int getAttendance = GetAttendedCount();
            lblPresent.Text = getAttendance.ToString();

            int getCount = GetPreReg();
            PreRegLbl.Text = getCount.ToString();


        }

        private void bunifuFlatButton6_Click(object sender, EventArgs e)
        {
            if (FinalFrame != null && FinalFrame.IsRunning == true)
            {
                FinalFrame.SignalToStop();
                FinalFrame.WaitForStop();
                FinalFrame.Stop();
                FinalFrame = null;
            }

        }

        private void bunifuImageButton4_Click(object sender, EventArgs e)
        {
            user_login ad = new user_login();
            Hide();
            ad.ShowDialog();
            Close();
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            DGVPrinter printer = new DGVPrinter();
            printer.Title = "STUDENT ATTENDANCE FORM";
            printer.SubTitle = "Example";
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.Footer = "Footer";
            printer.FooterSpacing = 15;

            printer.printDocument.DefaultPageSettings.Landscape = true;
            printer.PrintPreviewDataGridView(bunifuDataGridView2);
        }
    }
}
