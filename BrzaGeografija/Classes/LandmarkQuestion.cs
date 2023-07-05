using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrzaGeografija.Classes
{
    public class LandmarkQuestion : Question
    {
        List<Landmark> landmarks;

        public LandmarkQuestion(List<Landmark> _landmarks)
        {
            landmarks = _landmarks;
            questionId = FirebaseComm.getRandomNumber(landmarks.Count);
            answers = generateAnswers();
            imageUrl = landmarks.ElementAt(questionId).Image;
        }

        private List<int> generateAnswers()
        {
            List<int> ans = new List<int>();
            while (ans.Count != 4)
            {
                int rnd = FirebaseComm.getRandomNumber(landmarks.Count);
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
            answers.ForEach(a => ans.Add(landmarks.ElementAt(a).Name));
            return ans;
        }

        public override int getCorrectAnswer()
        {
            return correctAnswer;
        }

        public override string getQuestionText()
        {
            return "Koja е знаменитоста на сликата?";
        }

        public override string getImageUrl()
        {
            return landmarks.ElementAt(questionId).Image;
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
