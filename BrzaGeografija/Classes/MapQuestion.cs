using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrzaGeografija.Classes
{
    public class MapQuestion : Question
    {
        List<CountryMap> maps;

        public MapQuestion(List<CountryMap> _maps)
        {
            maps = _maps;
            questionId = FirebaseComm.getRandomNumber(maps.Count);
            answers = generateAnswers();
            imageUrl = maps.ElementAt(questionId).Png;
        }

        private List<int> generateAnswers()
        {
            List<int> ans = new List<int>();
            while (ans.Count != 4)
            {
                int rnd = FirebaseComm.getRandomNumber(maps.Count);
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
            answers.ForEach(a => ans.Add(maps.ElementAt(a).Country));
            return ans;
        }

        public override string getQuestionText()
        {
            return "Која е државата означена на мапата?";
        }

        public override string getImageUrl()
        {
            return maps.ElementAt(questionId).Png;
        }
    }
}
