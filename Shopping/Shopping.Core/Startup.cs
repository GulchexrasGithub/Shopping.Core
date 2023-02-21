// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shopping.Core.Brokers.Loggings;
using Shopping.Core.Brokers.Storages;
using Shopping.Core.Brokers.Tokens;
using Shopping.Core.Services.Foundations.Products;
using Shopping.Core.Services.Foundations.Users;
using Shopping.Core.Services.Orchestrations;
using Shopping.Core.Services.Processings;

namespace Shopping.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddDbContext<StorageBroker>();
            RegisterBrokers(services);
            AddFoundationServices(services);
            AddProcessingService(services);
            AddOrchestrationService(services);
            AddAuthenticationService(services);

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc(
                    name: "v1",
                    info: new OpenApiInfo { Title = "Shopping.Core", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(config => config.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    name: "Shopping.Core v1"));
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                endpoints.MapControllers());
        }

        private static void RegisterBrokers(IServiceCollection services)
        {
            services.AddTransient<IStorageBroker, StorageBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
            services.AddTransient<ITokenBroker, TokenBroker>();
        }

        private static void AddFoundationServices(IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ISecurityService, SecurityService>();
        }

        private void AddOrchestrationService(IServiceCollection services)
        {
            services.AddTransient<IUserSecurityOrchestrationService, UserSecurityOrchestrationService>();
        }

        private void AddProcessingService(IServiceCollection services)
        {
            services.AddTransient<IUserSecurityService, UserSecurityService>();
            services.AddTransient<IUserProcessingService, UserProcessingService>();
        }

        private void AddAuthenticationService(IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtOptions = Configuration.GetSection("JWTOptionsModel").Get<JWTOptionsModel>();

                    options
                        .TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateAudience = true,
                            ValidateIssuer = true,
                            ValidAudience = jwtOptions.Audience,
                            ValidIssuer = jwtOptions.Issuer,
                            IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET_KEY")))
                        };
                });
        }
    }
}