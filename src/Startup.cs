using Coravel;
using FileContextCore;
using Fluxmatix.AspNetCore.TagHelpers.QuillEditor;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Yazgelder.Config;
using Yazgelder.Entity;
using Yazgelder.Infrastructure;
using Yazgelder.Jobs;

namespace Yazgelder
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
#if DEBUG
            services.AddRazorPages()
            .AddRazorRuntimeCompilation();
#endif

            services.AddAuthentication("org.Yazgelder.Cookie")
                   .AddCookie("org.Yazgelder.Cookie", options =>
                   {
                       options.AccessDeniedPath = "/Admin/Login/";
                       options.LoginPath = "/Admin/Login/";
                   });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(30);
                options.Cookie.Name = "org.Yazgelder.Cookie";
            });

            services.AddDbContext<Context>(options =>
           options.UseFileContextDatabase(location: System.IO.Path.Combine(_env.WebRootPath, "data"))

           );

            services.AddOptions();
            services.Configure<MailConfig>(Configuration.GetSection("MailConfig"));
            services.Configure<GuvenSmsConfig>(Configuration.GetSection("GuvenSmsConfig"));
            services.Configure<MesajPaneliConfig>(Configuration.GetSection("MesajPaneliConfig"));
            services.Configure<ReportConfig>(Configuration.GetSection("ReportConfig"));
            services.Configure<SiteConfig>(Configuration.GetSection("SiteConfig"));
            services.AddScoped<ValidateReCaptchaAttribute>();
            services.AddScoped<ISysFunctions, SysFunctions>();
            services.AddQuillEditor();
            services.AddScheduler();
#if !DEBUG
             services.AddControllersWithViews();
#endif
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cultureInfo = new CultureInfo("tr-TR");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            var requestOpt = new RequestLocalizationOptions();
            requestOpt.SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("tr-TR")
                };
            requestOpt.SupportedUICultures = new List<CultureInfo>
                {
                    new CultureInfo("tr-TR")
                };
            requestOpt.RequestCultureProviders.Clear();
            requestOpt.RequestCultureProviders.Add(new SingleCultureProvider());
            app.UseRequestLocalization(requestOpt);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseQuillEditor();
            var provider = app.ApplicationServices;
            provider.UseScheduler(scheduler =>
            {
                scheduler.OnWorker("SmsTask");
                scheduler.Schedule<BirthdaySmsSenderJobs>()
                .DailyAtHour(10)
                .When(() => Task<bool>.FromResult(true))
                .Zoned(TimeZoneInfo.Local);
                scheduler.Schedule<SmsReportSmsSenderJobs>()
                .DailyAtHour(10)
                .Monday()
                .Zoned(TimeZoneInfo.Local);
            }).OnError((ex) =>
            {
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                     name: "areas",
                     areaName: "areas",
                     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }

        public class SingleCultureProvider : IRequestCultureProvider
        {
            public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
            {
                return Task.Run(() => new ProviderCultureResult("tr-TR", "tr-TR"));
            }
        }
    }
}