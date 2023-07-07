using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrzaGeografija.Classes
{
    class QuestionComparer : IEqualityComparer<Question>
    {
        public bool Equals(Question x, Question y)
        {
            return x.questionId == y.questionId;
        }

        public int GetHashCode(Question obj)
        {
            return obj.questionId;
        }
    }
}
