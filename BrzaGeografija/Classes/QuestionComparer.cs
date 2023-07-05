using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrzaGeografija.Classes
{
    class QuestionComparer : IEqualityComparer<IQuestion>
    {
        public bool Equals(IQuestion x, IQuestion y)
        {
            return x.getQuestionId() == y.getQuestionId();
        }

        public int GetHashCode(IQuestion obj)
        {
            return obj.getQuestionId();
        }
    }
}
