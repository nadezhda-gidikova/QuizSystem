using Quiz.Data;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly ApplicationDbContext context;

        public AnswerService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public int Add(string title, int points, bool isCorrect, int questionId)
        {
            var answer = new Answer
            {
                Title=title,
                Points=points,
                IsCorrect=isCorrect,
                QuestionId=questionId,
            };

            this.context.Answers.Add(answer);
            this.context.SaveChanges();

            return answer.Id;
        }
    }
}
