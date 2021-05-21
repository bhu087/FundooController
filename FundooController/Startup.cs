/////------------------------------------------------------------------------
////<copyright file="Startup.cs" company="BridgeLabz">
////author="Bhushan"
////</copyright>
////-------------------------------------------------------------------------

namespace FundooController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FundooManager.Account;
    using FundooManager.NotesManager;
    using FundooRepository.DbContexts;
    using FundooRepository.Repo.AccountRepository;
    using FundooRepository.Repo.NotesRepository;
    using LoggerService;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpsPolicy;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// Startup program
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration startup constructor
        /// </summary>
        /// <param name="configuration">cConfiguration parameter</param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Gets Configuration property
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configuration setting contains all service related settings
        /// </summary>
        /// <param name="services">services parameter</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<INotesManager, NotesManager>();
            services.AddTransient<INotesRepo, NotesRepo>();
            services.AddTransient<IAccountManager, AccountManager>();
            services.AddTransient<ILoggerManager, LoggerManager>();
            services.AddTransient<IAccountRepo, AccountRepo>();

            services.AddCors();
            services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddAuthentication("Bearer")
                    .AddJwtBearer("Bearer", options => options.SaveToken = true);

            services.AddDbContextPool<UserDbContext>(
               options => options.UseSqlServer(Configuration.GetConnectionString("UserDbConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <param name="env">Hosting environment</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSession();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Test Api v1");
            });
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
