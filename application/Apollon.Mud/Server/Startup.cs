using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Apollon.Mud.Server.DbContext;
using Apollon.Mud.Server.Domain.Implementations.Shared;
using Apollon.Mud.Server.Domain.Interfaces.Shared;
using Apollon.Mud.Server.Hubs.Implementations;
using Apollon.Mud.Server.Model.Implementations.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Apollon.Mud.Server
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
            services.AddIdentityCore<DungeonUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddSignInManager<SignInManager<DungeonUser>>()
                .AddEntityFrameworkStores<DungeonDbContext>();

            services.AddSingleton<IConnectionService, ConnectionService>();
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
        }
    }
}
