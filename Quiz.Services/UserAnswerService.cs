using Microsoft.EntityFrameworkCore;
using Quiz.Data;
using Quiz.Models;
using Quiz.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly ApplicationDbContext context;

        public UserAnswerService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void AddUserAnswer(string userName, int questionId, int answerId)
        {
            var userId = this.context.Users.Where(x => x.UserName == userName)
                .Select(x => x.Id).FirstOrDefault();
            var userAnswer = this.context.UserAnswers
                .FirstOrDefault(x => x.IdentityUserId == userId && x.QuestionId == questionId);
            userAnswer.AnswerId = answerId;
            this.context.SaveChanges();
        }
        
        //public void BulkAddUserAnswer(QuizInputModel quizInputModel)
        //{
        //    var userAnswers = new List<UserAnswer>();
        //    foreach (var item in quizInputModel.Questions)
        //    {

        //        var userAnswer = new UserAnswer
        //        {
        //            IdentityUserId = quizInputModel.UserId,
        //            QuizId = quizInputModel.QuizId,
        //            AnswerId=item.AnswerId,
        //            QuestionId=item.QuestionId
        //        };
        //        userAnswers.Add(userAnswer);
        //    }

        //    this.context.AddRange(userAnswers);
        //    this.context.SaveChanges();
        //}
        public int GetUserResult(string userName, int quizId)
        {
            var userId = this.context.Users.Where(x => x.UserName == userName)
                .Select(x => x.Id).FirstOrDefault();
            var totalPoints = this.context.UserAnswers
                .Where(x => x.IdentityUserId == userId
                        && x.Question.QuizId == quizId)
                .Sum(x => x.Answer.Points);
            return totalPoints;
        }
    }
}
