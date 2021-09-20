using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using net_project.Api.Api.Interfaces;
using net_project.Api.Api.Repositories;
using net_project.Api.Api.Settings;

namespace net_project.Api.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));
            
            var mongoDBSettings = Configuration.GetSection(nameof(MongoDBSettings)).Get<MongoDBSettings>();

            services.AddCors(options =>
            {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              builder =>
                              {
                                  // TODO: This enables any webservice to connect to the API.
                                  // Only to be used while developing
                                  
                                  builder.AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowAnyOrigin();
                              });
            });

            services.AddSingleton<IMongoClient>(ServiceProvider => 
            {
                return new MongoClient(mongoDBSettings.ConnectionString);
            });
            //services.AddSingleton<ItemsRepositoryInterface, InMemItemsRepository>();
            services.AddSingleton<ItemsRepositoryInterface, MongoDBItemsReposity>();


            services.AddControllers(options =>
                                    options.SuppressAsyncSuffixInActionNames = false);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "net_project.Api", Version = "v1" });
            });

            services.AddHealthChecks()
                    .AddMongoDb(mongoDBSettings.ConnectionString, 
                    name: "mongodb", 
                    timeout: TimeSpan.FromSeconds(3),
                    tags: new[] { "Ready" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpsRedirection();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "net_project.Api v1"));
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/Healtz/ready", new HealthCheckOptions{
                   Predicate = (check) => check.Tags.Contains("Ready"),
                   ResponseWriter = async(context, report) => 
                   {
                       var result = JsonSerializer.Serialize(
                           new 
                           {
                               status = report.Status.ToString(),
                               checks = report.Entries.Select(entry => new 
                               {
                                   name = entry.Key,
                                   status = entry.Value.Status.ToString(),
                                   exception = entry.Value.Exception != null ? entry.Value.Exception.Message: "none",
                                   duration = entry.Value.Duration.ToString()
                               })
                           }
                       );
                       context.Response.ContentType = MediaTypeNames.Application.Json;
                       await context.Response.WriteAsync(result);
                   } 
                });
                endpoints.MapHealthChecks("/Healtz/live", new HealthCheckOptions
                {
                    Predicate = (_) => false
                });
            });
        }
    }
}
