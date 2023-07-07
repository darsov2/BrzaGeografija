using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrzaGeografija.Classes
{
    public abstract class Question
    {
        public int questionId { get; set; }
        public List<int> answers { get; set; }
        public int correctAnswer { get; set; }
        public string imageUrl { get; set; }
        public string answerQuestion(int ans)
        {
            if (ans == correctAnswer)
            {
                return "correct" + correctAnswer + ".png";
            }
            else
            {
                return "incorrect" + correctAnswer + ans + ".png";
            }
        }
        public abstract List<string> getAnswers();
        public abstract string getQuestionText();
        public abstract string getImageUrl();
    }
}
