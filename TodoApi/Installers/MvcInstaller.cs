using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Services;

namespace TodoApi.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //Add FluentValidation and the filters
            services.AddMvc(options =>
            {
                options.Filters.Add(new ValidationFilter());
            }).AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "EGAPI ToDo API",
                    Description = "Experience time for João Veloso",
                    TermsOfService = new Uri("https://www.linkedin.com/in/joaongveloso/"),
                    Contact = new OpenApiContact
                    {
                        Name = "João Veloso",
                        Email = "jngveloso@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/joaongveloso/")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License by João",
                        Url = new Uri("https://www.linkedin.com/in/joaongveloso/")
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddMvc(option => option.EnableEndpointRouting = false);

            services.AddControllers();

            services.AddControllersWithViews();

            services.AddTransient<ITodoRepository, TodoRepository>();
            services.AddTransient<ITodosService, TodosService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<ITaskService, TaskService>();
           


        }
    }
}
