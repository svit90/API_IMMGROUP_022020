using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace api.immgroup.com
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
            //services.AddCors(c =>
            //{
            //    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()
            //                                .AllowAnyMethod()
            //                                .AllowAnyHeader());
            //});
            //services.AddMvc()
            //    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            //    .ConfigureApiBehaviorOptions(options =>
            //    {
            //        options.SuppressModelStateInvalidFilter = true;
            //    });
            services.AddControllers();
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
            services.Configure<IISOptions>(options =>
            {
                options.ForwardClientCertificate = false;
            });
            //services.AddHttpClient("system.immgroup.com", client =>
            //{
            //    //client.BaseAddress = new Uri("http://localhost:13579/");
            //    client.BaseAddress = new Uri("https://system.immgroup.com");
            //    client.DefaultRequestHeaders.Add("Accept", "application/x-www-form-urlencoded");
            //});
            //services.AddHttpClient("immgroup.com", client =>
            //{
            //    client.BaseAddress = new Uri("https://immgroup.com");
            //    client.DefaultRequestHeaders.Add("Accept", "application/x-www-form-urlencoded");
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(options => options.AllowAnyOrigin()
                                           .AllowAnyMethod()
                                           .AllowAnyHeader());

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
