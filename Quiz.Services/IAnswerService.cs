using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
   public  interface IAnswerService
    {
        int Add(string title,int questionId, bool isCorrect, int points);
    }
}
