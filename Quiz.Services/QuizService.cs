
using Microsoft.EntityFrameworkCore;
using Quiz.Data;
using Quiz.Models;
using Quiz.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quiz.Services
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext context;

        public QuizService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public int Add(string title)
        {
            var quiz = new Quiz.Models.Quiz
            {
                Title = title
            };
            this.context.Quizes.Add(quiz);
            this.context.SaveChanges();
            return quiz.Id;
        } 
        public QuizViewModel GetQuizById(int quizId)
        {
            var quiz = this.context.Quizes
                .Include(x=>x.Questions)
                .ThenInclude(x=>x.Answers)
                .FirstOrDefault(x => x.Id == quizId);

            var quizViewModel = new QuizViewModel
            {
                Id=quiz.Id,
                Title = quiz.Title,
                Questions = quiz.Questions.OrderBy(r=>Guid.NewGuid()).Select(x => new QuestionViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Answers = x.Answers.OrderBy(a=>Guid.NewGuid()).Select(a => new AnswerViewModel
                    {
                        Id = a.Id,
                        Title = a.Title
                    })
                    .ToList()
                })
                .ToList()
            };
            return quizViewModel;
        }

        public IEnumerable<UserQuizViewModel> GetQuizesByUserName(string userName)
        {
           var quizes= context.Quizes.Select(x => new UserQuizViewModel
            {
                QuizId = x.Id,
                Title = x.Title,
            }).ToList();

            foreach (var quiz in quizes)
            {
                var questionsCount = context.UserAnswers
                        .Count(ua => ua.IdentityUser.UserName == userName
                        && ua.Question.QuizId == quiz.QuizId);
                if (questionsCount==0)
                {
                    quiz.Status = QuizStatus.NotStarted;
                    continue;
                }

                var answeredQuestions = context.UserAnswers
                        .Count(ua => ua.IdentityUser.UserName == userName
                                && ua.Question.QuizId == quiz.QuizId
                                && ua.AnswerId.HasValue);
                if (answeredQuestions == questionsCount)
                {
                    quiz.Status = QuizStatus.Finished;
                }
                else
                {
                    quiz.Status = QuizStatus.InProgress;
                }

            }
            return quizes;
        }

        public void StartQuiz(string userName, int quizId)
        {
            if (context.UserAnswers.Any(x=>x.IdentityUser.UserName == userName
                &&x.Question.QuizId==quizId))
            {
               return;
            }
            var questions = context.Questions.Where(x=>x.QuizId==quizId)
                .Select(x => new { x.Id }).ToList();
            var userId = this.context.Users.Where(x => x.UserName == userName)
                .Select(x => x.Id).FirstOrDefault();

            foreach (var question in questions)
            {
                context.UserAnswers.Add(new UserAnswer
                {
                    AnswerId = null,
                    IdentityUserId = userId,
                    QuestionId = question.Id,
                });
            }
            context.SaveChanges();
        }
    }
}
