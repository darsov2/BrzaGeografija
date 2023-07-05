using BrzaGeografija.Classes;
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
    public partial class TrainingScreen : Form
    {
        public TrainingScreen()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            QuizQuestion qq = new QuizQuestion(3);
            qq.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            QuizQuestion qq = new QuizQuestion(1);
            qq.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            QuizQuestion qq = new QuizQuestion(0);
            qq.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            QuizQuestion qq = new QuizQuestion(2);
            qq.Show();
        }
    }
}
