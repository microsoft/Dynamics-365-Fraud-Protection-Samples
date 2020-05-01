// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Contoso.FraudProtection.ApplicationCore.Interfaces;
using Contoso.FraudProtection.ApplicationCore.Services;
using Contoso.FraudProtection.Infrastructure.Data;
using Contoso.FraudProtection.Infrastructure.Identity;
using Contoso.FraudProtection.Infrastructure.Logging;
using Contoso.FraudProtection.Infrastructure.Services;
using Contoso.FraudProtection.Web.Interfaces;
using Contoso.FraudProtection.Web.Services;
using System;
using Microsoft.Extensions.Hosting;
using Contoso.FraudProtection.Web.Middleware;

namespace Contoso.FraudProtection.Web
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Called automatically by .NET Core if the environment is "Development"
        /// </summary>
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            //Swap which line is commented to test with an in memory DB or the production DB.
            //If you switch to production DBs, you'll also need to add connection strings in appsettings.json or appsettings.Development.json (both will have the same effect locally).
            ConfigureInMemoryDBs(services);
            //UseProductionDBs(services);

            ConfigureServices(services);
        }

        /// <summary>
        /// Called automatically by .NET Core if the environment is "Production"
        /// </summary>
        public void ConfigureProductionServices(IServiceCollection services)
        {
            UseProductionDBs(services);

            ConfigureServices(services);
        }

        private void UseProductionDBs(IServiceCollection services)
        {
            services.AddDbContext<CatalogContext>(c => c.UseSqlServer(Configuration.GetConnectionString("CatalogConnection")));
            services.AddDbContext<AppIdentityDbContext>(c => c.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));
        }

        private void ConfigureInMemoryDBs(IServiceCollection services)
        {
            services.AddDbContext<CatalogContext>(c => c.UseInMemoryDatabase("Catalog"));
            services.AddDbContext<AppIdentityDbContext>(c => c.UseInMemoryDatabase("Identity"));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.LoginPath = "/Account/Signin";
                options.LogoutPath = "/Account/Signout";
                options.Cookie = new CookieBuilder
                {
                    IsEssential = true // required for auth to work without explicit user consent; adjust to suit your privacy policy
                };
            });

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            services.AddScoped<ICatalogService, CachedCatalogService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IBasketViewModelService, BasketViewModelService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<CatalogService>();
            services.Configure<CatalogSettings>(Configuration);
            services.AddSingleton<IUriComposer>(new UriComposer(Configuration.Get<CatalogSettings>()));

            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddTransient<IEmailSender, EmailSender>();
            
            services.Configure<TokenProviderServiceSettings>(Configuration.GetSection("FraudProtectionSettings:TokenProviderConfig"));
            services.AddScoped<ITokenProvider, TokenProviderService>();
            services.Configure<FraudProtectionSettings>(Configuration.GetSection("FraudProtectionSettings"));
            services.AddScoped<IFraudProtectionService, FraudProtectionService>();

            services.AddMemoryCache();

            services.AddControllersWithViews();
            services.AddSession();
        }

        // This method gets called by the runtime.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureExceptionHandler();
            if (env.IsDevelopment())
            {
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(routes => 
            {
                routes.MapControllerRoute(
                    name: "areaRoute",
                    pattern: "{area:exists}/{controller}/{action}/{id?}",
                    defaults: new { action = "Index" });

                routes.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapControllerRoute(
                    name: "api",
                    pattern: "{controller}/{id?}");
            });
        }
    }
}
