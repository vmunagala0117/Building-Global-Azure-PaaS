using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AZ_Paas_Demo.Data;
using Microsoft.EntityFrameworkCore;
using AZ_Paas_Demo.Data.Interfaces;
using AZ_Paas_Demo.Data.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using AZ_Paas_Demo.Data.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Logging;

namespace AZ_Paas_Demo
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
            //https://docs.microsoft.com/en-us/azure/azure-monitor/app/asp-net-core
            services.AddApplicationInsightsTelemetry(Configuration);

            services.Configure<AzureAd>(Configuration.GetSection("AzureAd"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.AddDbContext<azpaasdemodbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("JuiceDBConnection")));

            //add Redis Cache Service
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("RedisCacheConnection");
                options.InstanceName = "master";
            });

            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IJuiceService, JuiceService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IRoutingService, RoutingService>();
            services.AddTransient<IContextFactory, ContextFactory>();
            services.AddTransient<IQueueService, QueueService>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //so when a user is authenticated, he will carry a cookie on his machine to authenticate his request with.
            //also instantiated OpenId middleware and pass the parameters from the appsettings.json file
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddCookie()
            .AddOpenIdConnect(options =>
            {
                options.Authority = string.Format(Configuration["AzureAd:AadInstance"], Configuration["AzureAd:TenantId"]);
                options.ClientId = Configuration["AzureAd:ClientId"];
                options.CallbackPath = Configuration["AzureAd:AuthCallback"];
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Juice Store User API",
                    Description = "A simple api to add a new user for a new juice store",
                    TermsOfService = "None"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddAzureWebAppDiagnostics();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Juice Store User API");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
