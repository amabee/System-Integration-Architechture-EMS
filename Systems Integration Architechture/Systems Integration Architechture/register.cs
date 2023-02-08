using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Systems_Integration_Architechture
{
    public partial class register : Form
    {
        MySqlConnection connection = DBClass.connection;
        public register()
        {
            InitializeComponent();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            QRCoder.QRCodeGenerator QG = new QRCoder.QRCodeGenerator();
            var MyData = QG.CreateQrCode(txtID.Text, QRCoder.QRCodeGenerator.ECCLevel.H);
            var code = new QRCoder.QRCode(MyData);
            pictureBox2.Image = code.GetGraphic(100);

            try
            {
                MemoryStream ms = new MemoryStream();
                pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] Photo = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(Photo, 0, Photo.Length);

                connection.Open();

                string iquery = "INSERT INTO `student_info`(`unique_id`, `student_id`, `fname`, `lname`, `course`, `year`, `event_attending`, `pre_registered`, `qr_picture`) " +
                    "VALUES ('unique_id', @studentID, @firstname , @lastname , @course , @year_level , @event_attending , @registered , @qrpicture)";
                MySqlCommand commandDatabase = new MySqlCommand(iquery, connection);
                commandDatabase.CommandTimeout = 60;
                commandDatabase.Parameters.AddWithValue("@studentID", txtID.Text);
                commandDatabase.Parameters.AddWithValue("@firstname", txtFname.Text);
                commandDatabase.Parameters.AddWithValue("@lastname", txtLname.Text);
                commandDatabase.Parameters.AddWithValue("@course", courseSelector.SelectedItem.ToString());
                commandDatabase.Parameters.AddWithValue("@year_level", dropYear.SelectedItem.ToString());
                commandDatabase.Parameters.AddWithValue("@event_attending", eventSelector.SelectedItem);
                commandDatabase.Parameters.AddWithValue("@registered", "Pre-Registered");
                commandDatabase.Parameters.AddWithValue("@qrpicture", Photo);
                try
                {
                    int rowsAffected = commandDatabase.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        string initialDIR = @"C:\Users\Sho\Desktop\qrcodes";
                        string fileName = "qrcode_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                        string filePath = Path.Combine(initialDIR, fileName);
                        pictureBox2.Image.Save(filePath);

                        DialogResult msg = MessageBox.Show("Pre-Registered Successfully!", "Windows Alert!", MessageBoxButtons.OK);
                        if (msg == DialogResult.OK)
                        {
                            txtID.Text = "";
                            txtFname.Text = "";
                            txtLname.Text = "";
                            eventSelector.SelectedIndex = -1;
                            courseSelector.SelectedItem = "";
                            dropYear.SelectedItem = "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    MessageBox.Show(ex.Message);
                }

                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void addItemsToEventSelector()
        {

            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                string query = "SELECT `event_name`, `event_status` FROM `events` WHERE event_status = 'Ongoing'";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                using (MySqlDataReader _read = cmd.ExecuteReader())

                    while (_read.Read())
                {
                    eventSelector.Items.Add(_read.GetString("event_name"));
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


        private void register_Load(object sender, EventArgs e)
        {
            populateCourse();
            populateYearLevels();
            addItemsToEventSelector();

            if (eventSelector.SelectedIndex == -1)
            {
                txtID.Enabled = false;
                txtFname.Enabled = false;
                txtLname.Enabled = false;
            }

        }

        private void populateCourse()
        {
            courseSelector.Items.Add("BSIT");
            courseSelector.Items.Add("BSCRIM");
            courseSelector.Items.Add("BSEDUC");
            courseSelector.Items.Add("BSHRM");
            courseSelector.Items.Add("BS-Civil Engineering");
            courseSelector.Items.Add("BS-Pharma");
            courseSelector.Items.Add("BS-Nursing");
            courseSelector.Items.Add("BS-Electrical Engineering");
            courseSelector.Items.Add("BS-Mechanical Engineering");
            courseSelector.Items.Add("BA-COM");

        }

        private void populateYearLevels()
        {
            dropYear.Items.Add("1st Year");
            dropYear.Items.Add("2nd Year");
            dropYear.Items.Add("3rd Year");
            dropYear.Items.Add("4th Year");

        }

        private void eventSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventSelector.SelectedIndex == -1)
            {
                txtID.Enabled = false;
                txtFname.Enabled = false;
                txtLname.Enabled = false;
            }
            else
            {
                txtID.Enabled = true;
                txtFname.Enabled = true;
                txtLname.Enabled = true;
            }
        }
    }
}
