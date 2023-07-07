using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrzaGeografija.Classes
{
    public class CapitalQuestion : Question
    {
        static List<Country> countries;

        public CapitalQuestion(List<Country> _countries)
        {
            countries = _countries;
            questionId = FirebaseComm.getRandomNumber(countries.Count);
            answers = generateAnswers();
            imageUrl = countries.ElementAt(questionId).Png;
        }

        private List<int> generateAnswers()
        {
            List<int> ans = new List<int>();
            while(ans.Count != 4)
            {
                int rnd = FirebaseComm.getRandomNumber(countries.Count);
                if(ans.Contains(rnd) || rnd == questionId)
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
            answers.ForEach(a => ans.Add(countries.ElementAt(a).Capital));
            return ans;
        }

        public override string getQuestionText()
        {
            return "Кој е главен град на " + countries.ElementAt(questionId).Name;
        }

        public override string getImageUrl()
        {
            return countries.ElementAt(questionId).Png;
        }
    }
}
