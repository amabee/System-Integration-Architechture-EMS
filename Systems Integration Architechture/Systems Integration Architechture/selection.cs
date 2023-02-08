using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Systems_Integration_Architechture
{
    public partial class selection : Form
    {
        public selection()
        {
            InitializeComponent();
        }

        private void selection_Load(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            admin_login admin = new admin_login();
            Hide();
            admin.ShowDialog();
            Close();
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            user_login usr = new user_login();
            Hide();
            usr.ShowDialog();
            Close();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            register reg = new register();
            Hide();
            reg.ShowDialog();
            Close();
        }
    }
}
