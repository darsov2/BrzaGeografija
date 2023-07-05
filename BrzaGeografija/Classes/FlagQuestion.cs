using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrzaGeografija.Classes
{
    public class FlagQuestion : Question
    {
        List<Country> countries;

        public FlagQuestion(List<Country> _countries)
        {
            countries = _countries;
            questionId = FirebaseComm.getRandomNumber(countries.Count);
            answers = generateAnswers();
            imageUrl = countries.ElementAt(questionId).Png;
        }

        private List<int> generateAnswers()
        {
            List<int> ans = new List<int>();
            while (ans.Count != 4)
            {
                int rnd = FirebaseComm.getRandomNumber(countries.Count);
                if (ans.Contains(rnd) || rnd == questionId)
                {
                    continue;
                }
                ans.Add(rnd);
            }
            correctAnswer = FirebaseComm.getRandomNumber(4);
            ans[correctAnswer] = questionId;
            return ans;
        }
        public override List<string> getAnswers()
        {
            List<string> ans = new List<string>();
            answers.ForEach(a => ans.Add(countries.ElementAt(a).Name));
            return ans;
        }

        public override int getCorrectAnswer()
        {
            return correctAnswer;
        }

        public override string getQuestionText()
        {
            return "Чие е знамето на сликата?";
        }

        public override string getImageUrl()
        {
            return countries.ElementAt(questionId).Png;
        }

        public override int getQuestionId()
        {
            return questionId;
        }

        public override string answerQuestion(int ans)
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
    }
}
