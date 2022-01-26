
using Microsoft.EntityFrameworkCore;
using Quiz.Data;
using Quiz.Services.Models;
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
                Title = quiz.Title,
                Questions = quiz.Questions.Select(x => new QuestionViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Answers = x.Answers.Select(a => new AnswerViewModel
                    {
                        Id = a.Id,
                        Title = a.Title
                    })
                })
            };
            return quizViewModel;
        }

    }
}
