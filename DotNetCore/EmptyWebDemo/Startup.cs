using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace EmptyWebDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("test.html");
            app.UseDefaultFiles(options);
            app.UseStaticFiles();
            
            // app.UseDirectoryBrowser(new DirectoryBrowserOptions
            // {
            //     FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
            //     RequestPath = "/MyImages"
            // });

            // app.UseStaticFiles(new StaticFileOptions(){
            //     // FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"staticfiles")),
            //     // RequestPath = "/files"

            //     FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"MyStaticFiles")),
            //     RequestPath = new PathString("/StaticFiles")
            // });

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
                RequestPath = "/StaticFiles",
                EnableDirectoryBrowsing = true
            });
           
            app.Use(async(context,next)=>{
                context.Response.ContentType="text/html";
                await context.Response.WriteAsync("From First middleware...<br/>");
                await next.Invoke();
                await context.Response.WriteAsync("From First middleware.....<br/>");
            });

             app.Use(async(context,next)=>{
                await context.Response.WriteAsync("From Second middleware...<br/>");
                await next.Invoke();
                await context.Response.WriteAsync("From Second middleware.....<br/>");
            });

            app.Map(new PathString("/about"),(appBuilder)=>{
                appBuilder.Use(async(context,next)=>{
                    await context.Response.WriteAsync("From Third middleware...<br/>");
                    await next.Invoke();
                    await context.Response.WriteAsync("From Third middleware.....<br/>");
                });
                appBuilder.Run(async (context) =>
                {
                    await context.Response.WriteAsync("About Page!<br/>");
                });
            });

            app.MapWhen((context)=>context.Request.Query.ContainsKey("id"),(appBuilder)=>{
                appBuilder.Use(async(context,next)=>{
                    await context.Response.WriteAsync("From Query middleware...<br/>");
                    await next.Invoke();
                    await context.Response.WriteAsync("From Query middleware.....<br/>");
                });
                appBuilder.Run(async (context) =>
                {
                    await context.Response.WriteAsync("Page with ID!<br/>");
                });
            });

            app.UseMiddleware<MyCustomMiddleware>();
            app.UseMyCustomMiddleware();            

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!<br/>");
            });
        }
    }
}
