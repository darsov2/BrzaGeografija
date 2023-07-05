using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrzaGeografija.Classes
{
    public abstract class Question : IQuestion
    {
        public int questionId { get; set; }
        public List<int> answers { get; set; }
        public int correctAnswer { get; set; }
        public string imageUrl { get; set; }
        public abstract string answerQuestion(int ans);
        public abstract List<string> getAnswers();
        public abstract int getCorrectAnswer();
        public abstract string getImageUrl();
        public abstract int getQuestionId();
        public abstract string getQuestionText();
    }
}
