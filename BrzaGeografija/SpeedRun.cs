using BrzaGeografija.Classes;
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
    public partial class SpeedRun : Form
    {
        private readonly string alphabet = "АБВГДЕЖЗИЈКЛЉМНОПРСТЌУФХЦЧЏШ";
        private readonly Random random = new Random();
        private int time;
        private List<string> data;
        private int dataSize;
        private int correctAnswers;

        public SpeedRun(int questionType)
        {
            InitializeComponent();
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FileName = "";
            char letter = randomLetter();
            label3.Text = letter.ToString();
            if (questionType == 0)
            {
                data = FirebaseComm.FetchCities(letter);
                FileName = string.Format("{0}Resources\\" + "srGradovi.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            }
            else if(questionType == 1)
            {
                data = FirebaseComm.FetchCountries(letter);
                FileName = string.Format("{0}Resources\\" + "27.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            }
            else if(questionType == 2)
            {
                data = FirebaseComm.FetchMountains(letter);
                FileName = string.Format("{0}Resources\\" + "26.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            }
            else if(questionType == 3)
            {
                data = FirebaseComm.FetchRivers(letter);
                FileName = string.Format("{0}Resources\\" + "28.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            }
            this.BackgroundImage = Image.FromFile(FileName);
            time = 0;
        }

        private void SpeedRun_Load(object sender, EventArgs e)
        {
            time = 0;
            correctAnswers = 0;
            dataSize = data.Count;
            time = dataSize * 3;
            timerLabelFormat();
            pointsLabelFormat();
            timer1.Start();
        }

        private char randomLetter()
        {
            return alphabet[random.Next(alphabet.Length)];
        }

        private void timerLabelFormat()
        {
            label2.Text = new TimeSpan(0, time / 60, time % 60).ToString("mm':'ss");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time--;
            if(time <= 0)
            {
                timer1.Stop();
                Time time = new Time(data);
                time.Show();
            }
            timerLabelFormat();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        
        private void pointsLabelFormat()
        {
            label1.Text = correctAnswers + "/" + dataSize;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && data.Any(x => x.Equals(textBox1.Text, StringComparison.OrdinalIgnoreCase)))
            {
                listBox1.Items.Add(textBox1.Text);
                data.RemoveAll(x => x.Equals(textBox1.Text, StringComparison.OrdinalIgnoreCase));
                textBox1.Clear();
                correctAnswers++;
                pointsLabelFormat();
            }
        }
    }
}
