using Quiz.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public interface IUserAnswerService
    {
        void AddUserAnswer(string userId,  int questionId, int answerId);

        //public void BulkAddUserAnswer(QuizInputModel quizInputModel);

        int GetUserResult(string userId, int quizId);
    }
}
