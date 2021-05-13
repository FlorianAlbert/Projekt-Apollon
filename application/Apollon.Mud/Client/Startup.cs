using Apollon.Mud.Client.Data;
using Apollon.Mud.Client.Data.Account;
using Apollon.Mud.Client.Services.Implementations;
using Apollon.Mud.Client.Services.Interfaces;
using Apollon.Mud.Server.Outbound.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollon.Mud.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpClient("RestHttpClient", httpClient =>
            {
                httpClient.BaseAddress = new Uri(Configuration.GetSection("LoginConfiguration").GetSection("BaseUri").Value + ":" 
                    + Configuration.GetSection("LoginConfiguration").GetSection("Port").Value);

            });
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IDungeonService, DungeonService>();
            services.AddTransient<IAvatarService, AvatarService>();
            services.AddTransient<INpcService, NpcService>();
            services.AddTransient<IClassService, ClassService>();
            services.AddTransient<IRaceService, RaceService>();
            services.AddTransient<IRoomService, RoomService>();
            services.AddTransient<ISpecialActionService, SpecialActionService>();
            services.AddTransient<IInspectableService, InspectableService>();
            services.AddTransient<ITakeableService, TakeableService>();
            services.AddTransient<IConsumableService, ConsumableService>();
            services.AddTransient<IUsableService, UsableService>();
            services.AddTransient<IWearableService, WearableService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IUserService, UserService>();
            services.AddScoped<UserContext>();
            services.AddScoped<CustomAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthenticationStateProvider>());
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
  
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
