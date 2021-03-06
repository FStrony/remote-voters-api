using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using remotevotersapi.Application.AutoMapper;
using remotevotersapi.Application.Services;
using remotevotersapi.Infra.Data.Repositories;
using remotevotersapi.Infra.ModelSettings;
using remotevotersapi.Utils;

namespace remotevotersapi
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowCORS", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                });
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                // Configure a custom converter
                options.SerializerSettings.Converters.Add(new ObjectIdConverter());
            });

            services.AddAutoMapper();
            AutoMapperConfig.RegisterMappingMVC();
            // Register the MongoDBConfig in MongoDBConfig class
            services.Configure<MongoDBConfig>(Configuration.GetSection("MongoDBConfig"));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "RemoteVotersAPI",
                    Description = "RemoteVoters ASP.NET Core Web API",
                    TermsOfService = new Uri("https://github.com/FStrony/RemoteVotersAPI/blob/main/README.md"),
                    Contact = new OpenApiContact
                    {
                        Name = "Fernando Augusto Santos (FStrony)",
                        Email = "fstrony@gmail.com",
                        Url = new Uri("http://fstrony.github.io"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });

            services.AddScoped<VoteService, VoteService>();
            services.AddScoped<CampaignService, CampaignService>();
            services.AddScoped<CompanyService, CompanyService>();
            services.AddScoped<AuthService, AuthService>();

            services.AddScoped<CampaignRepository, CampaignRepository>();
            services.AddScoped<CompanyRepository, CompanyRepository>();
            services.AddScoped<VoteRepository, VoteRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c => { 
                c.SerializeAsV2 = true;
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RemoteVotersAPI V1");
                c.RoutePrefix = string.Empty;
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowCORS");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
