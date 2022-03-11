using AuthApp.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.AspNetCore3;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AuthApp
{
    public class Startup
    {
        // Set the TEST_WEB_CLIENT_SECRET_FILENAME configuration key to point to the client ID json file.
        // This can be set on appsettings.json or as an environment variable.
        // You can read more about configuring ASP.NET Core applications here:
        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1
        private const string ClientSecretFilenameKey = "TEST_WEB_CLIENT_SECRET_FILENAME";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();

            services
            .AddAuthentication(o =>
            {
                o.DefaultScheme = "Application";
                o.DefaultSignInScheme = "External";
            })
            .AddCookie("Application")
            .AddCookie("External")
            .AddGoogle(o =>
            {
                o.ClientId = "663662568199-3aprn1arng1esiejdt2uqgt3k2nam9bj.apps.googleusercontent.com";
                o.ClientSecret = "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";
            });
            // This configures Google.Apis.Auth.AspNetCore3 for use in this app.
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // This is not a production app so we always use the developer exception page.
            // You should ensure that your app uses the correct error page depending on the environment
            // it runs in.
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}