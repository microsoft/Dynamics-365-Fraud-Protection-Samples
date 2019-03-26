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
using System.Text;

namespace Contoso.FraudProtection.Web
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        private IServiceCollection _services;

        public Startup(IHostingEnvironment env, IConfiguration configuration)
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

            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);
            services.AddSession();
            _services = services;
        }

        // This method gets called by the runtime.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                ListAllRegisteredServices(app);
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Catalog/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes => 
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}/{id?}",
                    defaults: new { action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "api",
                    template: "{controller}/{id?}");
            });
        }

        private void ListAllRegisteredServices(IApplicationBuilder app)
        {
            app.Map("/allservices", builder => builder.Run(async context =>
            {
                var sb = new StringBuilder();
                sb.Append("<h1>All Services</h1>");
                sb.Append("<table><thead>");
                sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
                sb.Append("</thead><tbody>");
                foreach (var svc in _services)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{svc.ServiceType.FullName}</td>");
                    sb.Append($"<td>{svc.Lifetime}</td>");
                    sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table>");
                await context.Response.WriteAsync(sb.ToString());
            }));
        }
    }
}
