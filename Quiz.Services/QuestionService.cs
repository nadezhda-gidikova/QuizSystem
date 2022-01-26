using Quiz.Data;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext context;

        public QuestionService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public int Add(string title, int quizId)
        {
            var question = new Question()
            {
                Title = title,
                QuizId = quizId,
            };

            this.context.Questions.Add(question);
            this.context.SaveChanges();
            return question.Id;
        }
    }
}
