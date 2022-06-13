using Azure.Storage.Blobs;
using DocManager.DAL.Core.Context;
using DocManager.DAL.Entities;
using DocManager.DAL.Interfaces;
using DocManager.Services.Interfaces;
using DocManager.Services.Services;
using FileContextCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocManager
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
            //EF Core
            //services.AddDbContext<ApplicationDbContext>(options => {
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultDbConn"));
            //});

            services.AddDbContext<FileCoreDbContext>(options => options.UseFileContextDatabase());
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DocManager", Version = "v1" });
            });

            // Adding to the DI Container
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddSingleton(x => new BlobServiceClient(Configuration.GetValue<String>("AzureConn")));
            services.AddSingleton<IAzureBlobService, AzureBlobService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocManager v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
