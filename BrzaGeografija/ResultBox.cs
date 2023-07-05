using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrzaGeografija
{
    public partial class ResultBox : Form
    {
        public ResultBox(int correct, int total)
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            InitializeComponent();
            float percent = correct / (float)total * 100;
            label1.Text = correct + "/" + total;
            if (percent < 30)
            {
                string FileName = string.Format("{0}Resources\\" + "crveno.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
                pictureBox1.Image = Image.FromFile(FileName);
            }
            else if (percent >= 30 && percent < 60)
            {
                string FileName = string.Format("{0}Resources\\" + "zolto.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
                pictureBox1.Image = Image.FromFile(FileName);
            }
            else if (percent >= 60)
            {
                string FileName = string.Format("{0}Resources\\" + "zeleno.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
                pictureBox1.Image = Image.FromFile(FileName);
            }
        }

        private void ResultBox_Load(object sender, EventArgs e)
        {

        }
    }
}
