using System.Net;
using System.Net.Mail;
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
            });

            services.AddDbContext<DungeonDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DungeonDbConnection")));
            services.AddIdentityCore<DungeonUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
                    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
                })
                .AddRoles<IdentityRole>()
                .AddSignInManager<SignInManager<DungeonUser>>()
                .AddEntityFrameworkStores<DungeonDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IConnectionService, ConnectionService>();
            services.AddScoped<IChatService, ChatService>();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("JwtBearer").GetValue<string>("TokenSecret"))),
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Apollon.Mud API"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/hubs/chat");
            });

            app.AddDefaultUserRoles();
        }
    }
}
