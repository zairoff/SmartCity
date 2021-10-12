using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sport.API.Auth;
using Sport.API.Extensions;
using Sport.Infrastructure.Base;
using Sport.Infrastructure.Context;
using Sport.Service;
using Sport.Service.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sport.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSwaggerConfig();
            services.AddAuth(Configuration);
            services.AddAutoMapper(typeof(Startup));
            services.Configure<JwtConfig>(Configuration.GetSection("Jwt"));
            services.AddApplicationServices(CurrentEnvironment, Configuration);

            services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false)
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.IgnoreNullValues = true;
                    });

            services.AddCors(options =>
                    {
                        options.AddPolicy(Configuration["AppSettings:CORS"].ToString(), policy => policy.AllowAnyOrigin());
                    });

            services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnection"));
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // to do: change hardcoded string to smth
            app.UseCors(Configuration["AppSettings:CORS"]);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwaggerConfig();            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
