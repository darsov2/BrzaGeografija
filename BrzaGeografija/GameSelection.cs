﻿using System;
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
    public partial class GameSelection : Form
    {
        public GameSelection()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            TrainingScreen trainingScreen = new TrainingScreen();
            trainingScreen.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            SpeedRunCategories speedRunCategories = new SpeedRunCategories();
            speedRunCategories.Show();
        }
    }
}
