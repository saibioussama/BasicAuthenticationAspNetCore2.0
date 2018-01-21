using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicAuthenticationAspNetCore2.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasicAuthenticationAspNetCore2
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
            services.AddMvc(options=> {
                //options.Filters.Add(new RequireHttpsAttribute());
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase();
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options=>
                {
                    options.AccessDeniedPath = "/";
                    options.LoginPath = "/accounts/login";
                    options.LogoutPath = "/accounts/logout";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyForAdmins", option =>
                {
                    option.RequireAuthenticatedUser();
                    option.RequireRole("admin");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
