using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrzaGeografija
{
    public partial class SpeedRunCategories : Form
    {
        public SpeedRunCategories()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SpeedRun sr = new SpeedRun(1);
            sr.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            SpeedRun sr = new SpeedRun(3);
            sr.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            SpeedRun sr = new SpeedRun(0);
            sr.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            SpeedRun sr = new SpeedRun(2);
            sr.Show();
        }
    }
}
