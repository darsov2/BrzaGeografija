using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrzaGeografija.Classes;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using System.IO;

namespace BrzaGeografija
{
    public partial class QuizQuestion : Form
    {
        HashSet<IQuestion> questions;
        int question;
        int openedQuestions;
        int correctQuestions;
        public QuizQuestion(int typeOfQuestion)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            questions = new HashSet<IQuestion>(new QuestionComparer());
            if (typeOfQuestion == 0)
            {
                List<Country> countries = FirebaseComm.FetchCountries();
                while(questions.Count != countries.Count)
                {
                    questions.Add(new CapitalQuestion(countries));
                }
            }
            else if(typeOfQuestion == 1)
            {
                List<Country> countries = FirebaseComm.FetchCountries();
                while (questions.Count != countries.Count)
                {
                    questions.Add(new FlagQuestion(countries));
                }
            }
            question = 0;
            openedQuestions = 0;
            correctQuestions = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void QuizQuestion_Load(object sender, EventArgs e)
        {
            loadQuestion(question);
        }

        private void answerQuestion(int ans)
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            string FileName = string.Format("{0}Resources\\" + questions.ElementAt(question - 1).answerQuestion(ans), Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            this.BackgroundImage = Image.FromFile(FileName);

            if(!FileName.Contains("incorrect"))
            {
                correctQuestions++;
            }
        }

        private void loadQuestion(int questionNo)
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            MessageBox.Show(Path.Combine(RunningPath, @"..\..\"));
            string FileName = string.Format("{0}Resources\\Blue Illustration World Meteorological Day Instagram Post (3).png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
            this.BackgroundImage = Image.FromFile(FileName);

            label1.Text = (questions.ElementAt(questionNo)).getQuestionText();

            List<string> answer = (questions.ElementAt(questionNo)).getAnswers();
            label2.Text = answer.ElementAt(0);
            label3.Text = answer.ElementAt(1);
            label4.Text = answer.ElementAt(2);
            label5.Text = answer.ElementAt(3);

            pictureBox1.Load(questions.ElementAt(questionNo).getImageUrl());

            question++;
            openedQuestions++;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            pictureBox2_Click(sender, e);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            loadQuestion(question);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            answerQuestion(0);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            answerQuestion(1);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            answerQuestion(2);
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            answerQuestion(3);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            pictureBox3_Click(sender, e);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            pictureBox4_Click(sender, e);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            pictureBox5_Click(sender, e);
        }
    }
}
