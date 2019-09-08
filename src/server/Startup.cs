using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using server.Mappers;
using server.Models;
using server.Repositories;

namespace server
{
    public class Startup
    {
        public static string ConnectionString;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("goats");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen();

            // Repositories dependency injection setup
            services.AddSingleton<IRepository<Authority>, AuthorityRepository>();
            services.AddSingleton<IRepository<Book>, BookRepository>();
            services.AddSingleton<IRepository<Commentary>, CommentaryRepository>();
            services.AddSingleton<IRepository<CommentaryBook>, CommentaryBookRepository>();
            services.AddSingleton<IRepository<Ranking>, RankingRepository>();
            services.AddSingleton<IRepository<Seminary>, SeminaryRepository>();

            // Mappers dependency injection setup
            services.AddSingleton<IMapper<Authority>, AuthorityMapper>();
            services.AddSingleton<IMapper<Book>, BookMapper>();
            services.AddSingleton<IMapper<Commentary>, CommentaryMapper>();
            services.AddSingleton<IMapper<CommentaryBook>, CommentaryBookMapper>();
            services.AddSingleton<IMapper<Ranking>, RankingMapper>();
            services.AddSingleton<IMapper<Seminary>, SeminaryMapper>();

            services.AddCors(options =>
            {
                options.AddPolicy("Default", builder =>
                {
                    builder.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                    ;
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("Default");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // app.UseHttpsRedirection();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
