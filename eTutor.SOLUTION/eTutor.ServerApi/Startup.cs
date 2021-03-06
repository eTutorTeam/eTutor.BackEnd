using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using eTutor.Core.Contracts;
using eTutor.Core.Managers;
using eTutor.Core.Models;
using eTutor.Core.Models.Configuration;
using eTutor.Core.Repositories;
using eTutor.FileHandler;
using eTutor.MailService;
using eTutor.Persistence;
using eTutor.Persistence.Repositories;
using eTutor.ServerApi.Helpers;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
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
                    opts.UseMySql(Configuration.GetConnectionString("AzureConnection"));
                });


            AuthenticationServiceConfiguration(services);
            
            ConfigureRepositories(services);
            
            ConfigureManagers(services);

            ConfigureContractServices(services);

            SetupConfigurationServices(services);

            services.AddAutoMapper();
            
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opts =>
                    {
                        opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    });

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
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

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //config.IncludeXmlComments(xmlPath);
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => 
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .Build());
            });
        }


        private void SetupConfigurationServices(IServiceCollection services)
        {
            var smpt = Configuration.GetSection("SMTP").Get<SMTPConfiguration>();
            services.AddScoped(typeof(SMTPConfiguration), s => smpt);
            //MailKit Configuration
            services.AddMailKit(optsBuilder =>
            {
                optsBuilder.UseMailKit(new MailKitOptions
                {
                    Account = smpt.User,
                    Password = smpt.Password,
                    Port = smpt.Port,
                    Server = smpt.Server,
                    SenderName = smpt.Name,
                    SenderEmail = smpt.Email,
                    Security = true
                });
            });

            var firebaseConfiguration = Configuration.GetSection("Firebase").Get<FirebaseConfiguration>();
            services.AddScoped(typeof(FirebaseConfiguration), fc => firebaseConfiguration);
            
            AppBaseRoute route = new AppBaseRoute {BasePath = Directory.GetCurrentDirectory()};
            services.AddScoped(typeof(AppBaseRoute), t => route);

            var emailLinksConfiguration = Configuration.GetSection("EmailLinks").Get<EmailLinksConfiguration>();
            services.AddScoped(typeof(EmailLinksConfiguration), elc => emailLinksConfiguration);
            
            
            string jsonPath = Path.Combine(route.BasePath, "etutorfirebaseadmin.json");
            var configurationJson = File.ReadAllText(jsonPath);
            if (FirebaseApp.DefaultInstance == null) { FirebaseApp.Create(new AppOptions {Credential = GoogleCredential.FromJson(configurationJson)}); }
            var instance = FirebaseMessaging.DefaultInstance;
            services.AddSingleton(typeof(FirebaseMessaging), instance);

        }

        private void ConfigureContractServices(IServiceCollection services)
        {
            services.AddScoped<IMailService, SMTPMailService>();
            services.AddScoped<IFileService, FirebaseStorageFileService>();
            services.AddScoped<INotificationService, PushNotificationService.PushNotificationService>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<ITutorSubjectRepository, TutorSubjectRepository>();
            services.AddScoped<IParentStudentRepository, ParentStudentRepository>();
            services.AddScoped<IChangePasswordRepository, ChangePasswordRepository>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IEmailValidationRepository, EmailValidationRepository>();
            services.AddScoped<IMeetingRepository, MeetingRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IParentAuthorizationRepository, ParentAuthorizationRepository>();
            services.AddScoped<IRejectedMeetingRepository, RejectedMeetingRepository>();
        }

        private void ConfigureManagers(IServiceCollection services)
        {
            services.AddScoped<UsersManager, UsersManager>();
            services.AddScoped<SubjectsManager, SubjectsManager>();
            services.AddScoped<TutorsManager, TutorsManager>();
            services.AddScoped<ParentsManager, ParentsManager>();
            services.AddScoped<AccountsManager, AccountsManager>();
            services.AddScoped<TutorSubjectsManager, TutorSubjectsManager>();
            services.AddScoped<DevicesManager, DevicesManager>();
            services.AddScoped<NotificationManager, NotificationManager>();
            services.AddScoped<MeetingsManager, MeetingsManager>();
            services.AddScoped<RatingManager, RatingManager>();
            services.AddScoped<ParentAuthorizationManager, ParentAuthorizationManager>();
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
