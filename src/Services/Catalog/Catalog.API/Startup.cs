using Catalog.API.Dto;
using Catalog.API.Infrastructure.DataContexts;
using Catalog.API.Infrastructure.Respositories;
using Catalog.Domain.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Serilog;
using System;

namespace Catalog.API
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
            services.AddControllers()
                .AddNewtonsoftJson(options => options.UseMemberCasing());

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "oink-oink - Catalog HTTP API",
                    Version = "v1",
                    Description = "The Catalog/books Microservice HTTP API"
                });
            });

            services.Configure<CatalogSettings>(Configuration);

            services.AddTransient<ICatalogBooksRepository, CatalogBooksRepository>();
            services.AddTransient<CatalogBooksReadDataContext>();

            BsonClassMap.RegisterClassMap<Product>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
                //cm.MapMember(c => c.Images).SetSerializer(new ImagesSerializer());
            });

            BsonClassMap.RegisterClassMap<Dto.Book>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSerilogRequestLogging();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
