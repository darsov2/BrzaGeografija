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
        HashSet<Question> questions;
        int question;
        int openedQuestions;
        int correctQuestions;
        int typeOfQuestion;
        public QuizQuestion(int typeOfQuestion)
        {
            InitializeComponent();
            this.typeOfQuestion = typeOfQuestion;
            this.DoubleBuffered = true;

            questions = new HashSet<Question>(new QuestionComparer());
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
            else if(typeOfQuestion == 2)
            {
                List<Landmark> landmarks = FirebaseComm.FetchLandmarks();
                while (questions.Count != landmarks.Count)
                {
                    questions.Add(new LandmarkQuestion(landmarks));
                }
            }
            LoadBackground();
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

        private void LoadBackground()
        {
            string RunningPath = AppDomain.CurrentDomain.BaseDirectory;
            if (typeOfQuestion == 0)
            {
                string FileName = string.Format("{0}Resources\\" + "gradovi.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
                string FileName2 = string.Format("{0}Resources\\" + "gradovi1.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
                this.BackgroundImage = Image.FromFile(FileName);
                pictureBox6.Image = Image.FromFile(FileName2);
            }
            else if(typeOfQuestion == 1)
            {
                string FileName = string.Format("{0}Resources\\" + "3.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
                string FileName2 = string.Format("{0}Resources\\" + "zname1.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
                this.BackgroundImage = Image.FromFile(FileName);
                pictureBox6.Image = Image.FromFile(FileName2);
            }    
            else if(typeOfQuestion == 2)
            {
                string FileName = string.Format("{0}Resources\\" + "4.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
                string FileName2 = string.Format("{0}Resources\\" + "znamenitosti.png", Path.GetFullPath(Path.Combine(RunningPath, @"..\..\")));
                this.BackgroundImage = Image.FromFile(FileName);
                pictureBox6.Image = Image.FromFile(FileName2);
            }
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

        private void adjustFontSize()
        {
            if(label1.Text.Length > 36)
            {
                label1.Font = new Font(label1.Font.FontFamily, 22, label1.Font.Style);
            }
            if (label2.Text.Length > 11)
            {
                label2.Font = new Font(label2.Font.FontFamily, 18, label2.Font.Style);
            }
            if (label3.Text.Length > 11)
            {
                label3.Font = new Font(label3.Font.FontFamily, 18, label3.Font.Style);
            }
            if (label4.Text.Length > 11)
            {
                label4.Font = new Font(label4.Font.FontFamily, 18, label4.Font.Style);
            }
            if (label5.Text.Length > 11)
            {
                label5.Font = new Font(label5.Font.FontFamily, 18, label5.Font.Style);
            }
        }

        private void setDefaultFontSize()
        {
            label1.Font = new Font(label1.Font.FontFamily, 28, label1.Font.Style);
            label2.Font = new Font(label2.Font.FontFamily, 28, label2.Font.Style);
            label3.Font = new Font(label3.Font.FontFamily, 28, label3.Font.Style);
            label4.Font = new Font(label4.Font.FontFamily, 28, label4.Font.Style);
            label5.Font = new Font(label5.Font.FontFamily, 28, label5.Font.Style);
        }

        private void loadQuestion(int questionNo)
        {
            LoadBackground();
            setDefaultFontSize();
            label1.Text = (questions.ElementAt(questionNo)).getQuestionText();

            List<string> answer = (questions.ElementAt(questionNo)).getAnswers();
            label2.Text = answer.ElementAt(0);
            label3.Text = answer.ElementAt(1);
            label4.Text = answer.ElementAt(2);
            label5.Text = answer.ElementAt(3);

            adjustFontSize();

            try
            {
                pictureBox1.Load(questions.ElementAt(questionNo).getImageUrl());
            }
            catch (Exception)
            {
                loadQuestion(++question);

            }


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

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            loadQuestion(question);
        }

        private void QuizQuestion_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResultBox resultBox = new ResultBox(correctQuestions, openedQuestions);
            resultBox.Show();
        }
    }
}
