using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using mvc1.Models;

namespace mvc1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
           // Configuration["EnableDeveloperExceptions"] = "true";//let' set it from appsettings.json
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<IdentityUser, IdentityRole>().
                    AddEntityFrameworkStores<BlogDataContext>();

            services.AddTransient<FormattingService>();
            services.AddControllersWithViews();
            services.AddTransient<FeatureToggles>(x => new FeatureToggles{
                DeveloperExceptions = Configuration.GetValue<bool>("FeatureToggles:DeveloperExceptions")
            });
            services.AddDbContext<BlogDataContext>(options =>
            {
                var connectionString = Configuration.GetConnectionString("BlogDataContext");
                options.UseSqlServer(connectionString);
            });
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
        IApplicationBuilder app,
         IWebHostEnvironment env,
         FeatureToggles features)
        {
            app.UseExceptionHandler("/error.html");
            //if (env.IsDevelopment())
            //if(Configuration.GetValue<bool>("EnableDeveloperExceptions"))
            
            if(features.DeveloperExceptions)
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.Use(async(context , next)=>{
                if(context.Request.Path.Value.StartsWith("/invalid"))
                {
                    throw new Exception("My error");
                }

                await next();
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();

            
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseFileServer();
        }

    }
}