using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventAPI.Infrastructure;
using EventAPI.Models;
using EventAPI.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using EventAPI.Exceptions;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc.Formatters;
using EventAPI.CustomFormatters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventAPI
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
            services.AddDbContext<EventDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            });

            services.AddScoped<IEventRepository<EventData>, EventRepository<EventData>>();
            services.AddScoped<IEventRepository<EventUser>, EventRepository<EventUser>>();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info
                {
                    Title="Envent Management API",
                    Version="1.0",
                    Contact=new Contact { Name="Siva",Email="mail@email.com"},
                    Description="API"
                });
            });

            services.AddCors();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer=true,
                        ValidateAudience=true,
                        ValidateLifetime=true,
                        ValidateIssuerSigningKey=true,
                        ValidIssuer=Configuration.GetValue<string>("Jwt:Issuer"),
                        ValidAudience=Configuration.GetValue<string>("Jwt:Audience"),
                        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Jwt:Secret")))
                    };
                });

            services.AddMvc(options => {
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
                //options.OutputFormatters.Insert(0, new CsvOutputFormatter());
            })
                .AddXmlDataContractSerializerFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.ConfigureExectionHandler();
            }

            app.UseAuthentication();

            // Client Side Exceptions
            //app.UseStatusCodePages();
            //app.UseStatusCodePages("application/json", "{0}, Client side error occured.");
            //app.UseStatusCodePagesWithRedirects("/home/status/{0}");
            //app.UseStatusCodePagesWithReExecute("/home/status/{0}");
            app.UseCors(config=>
            {
                config.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>{
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Envent Management API");
                options.RoutePrefix = "";
            });

            app.UseMvc();
        }
    }
}
