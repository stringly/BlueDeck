using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BlueDeck.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using BlueDeck.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BlueDeck.Models.Auth;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Reflection;
using System;
using System.IO;

namespace BlueDeck
{
    /// <summary>
    /// Startup Class
    /// </summary>
    public class Startup {
        IConfigurationRoot Configuration;
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">An <see cref="IHostingEnvironment"/> object.</param>
        public Startup(IHostingEnvironment env) {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json").Build();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940   
        /// </summary>
        /// <param name="services">An <see cref="IServiceCollection"/></param>
        public void ConfigureServices(IServiceCollection services) {
            
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:OrgChartComponents:ConnectionString"]));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddAuthentication(Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme);
            
            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info {
                    Title = "BlueDeck API",
                    Version = "v1",
                    Description = "A RESTful service to access BlueDeck Organization Information",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = "Jason Smith",
                        Email = "Admin@BlueDeck.net"
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "CanEditUser",
                    policyBuilder => policyBuilder.AddRequirements(
                        new CanEditUserRequirement()));
                options.AddPolicy(
                    "CanEditComponent",
                    policyBuilder => policyBuilder.AddRequirements(
                        new CanEditComponentRequirement()));
                options.AddPolicy(
                    "CanEditPosition",
                    policyBuilder => policyBuilder.AddRequirements(
                        new CanEditPositionRequirement()));
                options.AddPolicy(
                    "IsGlobalAdmin",
                    policyBuilder => policyBuilder.AddRequirements(
                        new IsGlobalAdminRequirement()));
            });
            services.AddScoped<IClaimsTransformation, ClaimsLoader>();
            services.AddSingleton<IAuthorizationHandler, IsGlobalAdminForUserHandler>();
            services.AddSingleton<IAuthorizationHandler, IsOwnerHandler>();
            services.AddSingleton<IAuthorizationHandler, IsMemberSupervisorHandler>();
            services.AddSingleton<IAuthorizationHandler, IsGlobalAdminForComponentHandler>();
            services.AddSingleton<IAuthorizationHandler, IsComponentSupervisorHandler>();
            services.AddSingleton<IAuthorizationHandler, CanCreateComponent>();
            services.AddSingleton<IAuthorizationHandler, IsPositionComponentSupervisorHandler>();
            services.AddSingleton<IAuthorizationHandler, IsGlobalAdminForPositionHandler>();
            services.AddSingleton<IAuthorizationHandler, CanCreatePositionHandler>();
            services.AddSingleton<IAuthorizationHandler, IsGlobalAdmin>();

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">An <see cref="IApplicationBuilder"/> object.</param>
        /// <param name="env">An <see cref="IHostingEnvironment"/> object.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePagesWithReExecute("/Error/Error", "?statusCode={0}");
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlueDeck API V1");
                c.InjectStylesheet("/swagger/ui/custom.css");
            });

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}");
                //routes.MapRoute(
                //    name: "Edit Member Route",
                //    template: "{controller=Members}/{action=Index}/{id}/{returnUrl}");
            });
        }
    }
}
