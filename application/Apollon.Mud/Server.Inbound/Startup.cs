using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Apollon.Mud.Server.Domain.DbContext;
using Apollon.Mud.Server.Domain.Implementations.Chat;
using Apollon.Mud.Server.Domain.Implementations.Shared;
using Apollon.Mud.Server.Domain.Interfaces.Chat;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Outbound.Hubs;
using Apollon.Mud.Server.Model.Implementations.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Apollon.Mud.Server.Domain.Implementations.UserManagement;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Apollon.Mud.Server.Inbound
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Apollon.Mud API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                    }
                });
            });

            services.AddDbContext<DungeonDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlite(
                    Configuration.GetConnectionString("DungeonDbConnection"),
                    optionsBuilder => optionsBuilder.MigrationsAssembly("Apollon.Mud.Server.Domain"));
                options.ConfigureWarnings(e => e.Log(CoreEventId.DetachedLazyLoadingWarning));
            });

            services.AddIdentityCore<DungeonUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
                    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
                })
                .AddRoles<IdentityRole>()
                .AddSignInManager<SignInManager<DungeonUser>>()
                .AddEntityFrameworkStores<DungeonDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IConnectionService, ConnectionService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserDbService, UserDbService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGameDbService, GameDbService>();
            services.AddSingleton<ITokenService, TokenService>();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtBearer:TokenSecret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            var email = Configuration.GetSection("FluentEmail").GetValue<string>("Email");
            var host = Configuration.GetSection("FluentEmail").GetValue<string>("Host");
            var port = Configuration.GetSection("FluentEmail").GetValue<int>("Port");

            services
                .AddFluentEmail(email)
                .AddRazorRenderer()
                .AddSmtpSender(host, port);

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Apollon.Mud API");
            });

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/hubs/chat");
                endpoints.MapHub<GameHub>("/hubs/game");
            });

            using var dbContext = serviceProvider.GetService<DungeonDbContext>();
            dbContext.Database.EnsureCreated();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in Enum.GetNames<Roles>())
            {
                if (!roleManager.RoleExistsAsync(role).Result) roleManager.CreateAsync(new IdentityRole(role)).Wait();
            }
        }
    }
}
