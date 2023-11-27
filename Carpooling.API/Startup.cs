using System;
using Carpooling.API.Extension;
using Carpooling.Data;
using Carpooling.Services;
using Carpooling.Services.Contracts;
using Carpooling.Services.Services;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Carpooling.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var variable = Environment.GetEnvironmentVariable("LocalSQLServer", EnvironmentVariableTarget.Machine);
            services.AddDbContext<CarpoolingContext>(options =>
            {
                options.UseSqlServer(variable);
            });

            Cloudinary cloudinary = new Cloudinary(
                new Account(
                    this.Configuration.GetSection("Cloudinary").GetValue<string>("cloudName"),
                    this.Configuration.GetSection("Cloudinary").GetValue<string>("apiKey"),
                    this.Configuration.GetSection("Cloudinary").GetValue<string>("apiSecret")
                    ));
            services.AddSingleton(cloudinary);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<ITripCandidateService, TripCandidateService>();
            services.AddScoped<ILoginService, LoginService>();

            services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Carpooling.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Carpooling.API v1"));
            }

            app.UseExceptionHandler(CustomeWebExtention.HandleExceptions());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
