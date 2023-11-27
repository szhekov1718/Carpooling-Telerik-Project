using System;
using Carpooling.Data;
using Carpooling.MVC.Models;
using Carpooling.Services;
using Carpooling.Services.Contracts;
using Carpooling.Services.DTO.IncomeDTOs;
using Carpooling.Services.Services;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Carpooling.MVC
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

            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(1000);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            Cloudinary cloudinary = new Cloudinary(
                new Account(
                    this.Configuration.GetSection("Cloudinary").GetValue<string>("cloudName"),
                    this.Configuration.GetSection("Cloudinary").GetValue<string>("apiKey"),
                    this.Configuration.GetSection("Cloudinary").GetValue<string>("apiSecret")
                    ));
            services.AddSingleton(cloudinary);

            services.AddScoped<UserDTO>();
            services.AddScoped<ModelMapper>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<ITripCandidateService, TripCandidateService>();
            services.AddScoped<ILoginService, LoginService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});
        }
    }
}
