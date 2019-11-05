using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eTutor.Core.Models;
using eTutor.Persistence;
using eTutor.Persistence.Seeders;
using eTutor.ServerApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace eTutor.ServerApi
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

            services.AddDbContext<ETutorContext>(opts =>
                {
                    opts.UseMySql(Configuration.GetConnectionString("MainConnection"));
                });
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .Build());
            });
            
            
            AuthenticationServiceConfiguration(services);
            
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opts =>
                    {
                        opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    });

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "eTutor Web Api",
                    Description = "Our web API"
                });
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field", 
                    Name = "Authorization", 
                    Type = SecuritySchemeType.ApiKey
                });
                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    Keys = { }
                });
            });
        }

        private void AuthenticationServiceConfiguration(IServiceCollection services)
        {
            services.AddIdentity<User, Role>(opts =>
                {
                    opts.Password.RequireDigit = false;
                    opts.Password.RequiredLength = 4;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<ETutorContext>()
                .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();
            services
                .AddAuthentication(opts =>
                {
                    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration.GetSection("JWT")["Issuer"],
                        ValidAudience = Configuration.GetSection("JWT")["Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWT")["Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
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
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "eTutor V1");
                config.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseMiddleware<HttpExceptionHandler>();
            
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            
            app.UseMvc();
        }
    }
}
