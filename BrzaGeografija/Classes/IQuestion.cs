using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrzaGeografija.Classes
{
    interface IQuestion
    {
        string getQuestionText();
        List<string> getAnswers();
        int getCorrectAnswer();
        string getImageUrl();
        int getQuestionId();
        string answerQuestion(int ans);
    }
}
