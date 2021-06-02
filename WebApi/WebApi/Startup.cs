using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi
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
            //#if DEBUG
            //            var msiEnvironment = new MSIEnvironmentSettings();
            //            Configuration.Bind("MSIEnvironment", msiEnvironment);
            //            Environment.SetEnvironmentVariable("AZURE_CLIENT_ID", msiEnvironment.ClientId);
            //            Environment.SetEnvironmentVariable("AZURE_TENANT_ID", msiEnvironment.TenantId);
            //            Environment.SetEnvironmentVariable("AZURE_CLIENT_SECRET", msiEnvironment.ClientSecret);
            //#endif
           
            var blobStorageSettings = new BlobStorageSettings();
            Configuration.Bind("BlobStorageSettings", blobStorageSettings);
            services.AddSingleton<BlobStorageSettings>(blobStorageSettings);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", option => option.AllowAnyOrigin());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseCors(x => x
                     .AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader());

            app.UseHttpsRedirection();

            var options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("index.html");

            app.UseDefaultFiles(options);
            app.UseStaticFiles();

            app.UseRouting();

 

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
