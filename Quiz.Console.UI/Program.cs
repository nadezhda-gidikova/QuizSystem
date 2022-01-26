using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quiz.Data;
using Quiz.Services;
using System.IO;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Quiz.Console.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();


            var json = File.ReadAllText("EF-Core-Quiz.json");
            var questions = JsonConvert.DeserializeObject<IEnumerable<JsonQuestion>>(json);



            var quizService = serviceProvider.GetService<IQuizService>();
            var questionService = serviceProvider.GetService<IQuestionService>();
            var answerService = serviceProvider.GetService<IAnswerService>();

            var quizId = quizService.Add("EF Core Test");

            foreach (var item in questions)
            {
                var questionId = questionService.Add(item.Question, quizId);
                foreach (var answer in item.Answers)
                {
                    answerService.Add(answer.Answer, answer.Correct ? 1 : 0, answer.Correct, questionId);
                }

            }


            //var quizService = serviceProvider.GetService<IUserAnswerService>();
            //System.Console.WriteLine(quizService.GetUserResult("39b9d5ab-b116-4717-929e-81ed362ed925", 1)); 
            //quizService.Add("C# DB");
            //var quiz = quizService.GetQuizById(1);
            //System.Console.WriteLine(quiz.Title);

            //foreach (var question in quiz.Questions)
            //{
            //    System.Console.WriteLine("-"+question.Title);
            //    foreach (var answer in question.Answers)
            //    {
            //        System.Console.WriteLine("--"+answer.Title);
            //    }
            //}

            //var questionService = serviceProvider.GetService<IQuestionService>();
            //questionService.Add("1+1", 1);
            //questionService.Add("What is Entity Framework Core",1);

            //var answerService = serviceProvider.GetService<IAnswerService>();
            //answerService.Add("It is ORM", 5, true, 2);

            //var userAnswerService = serviceProvider.GetService<IUserAnswerService>();
            //userAnswerService.AddUserAnswer("39b9d5ab-b116-4717-929e-81ed362ed925", 1, 5, 1);

            //var quizService = serviceProvider.GetService<IUserAnswerService>();
            //var quiz = quizService.GetUserResult("39b9d5ab-b116-4717-929e-81ed362ed925", 1);

            //Console.WriteLine(quiz);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            services.AddDbContext<ApplicationDbContext>(options
                => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
              .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IQuizService, QuizService>();
            services.AddTransient<IQuestionService, QuestionService>();
            services.AddTransient<IAnswerService, AnswerService>();
            services.AddTransient<IUserAnswerService, UserAnswerService>();
            
        }
    }
}
