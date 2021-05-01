using System;
using Apollon.Mud.Server.Model.Implementations.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    /// <summary>
    /// ToDo
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddDefaultUserRoles(this IApplicationBuilder app)
        {
            var roleManager = app.ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in Enum.GetNames<Roles>())
            {
                if (!roleManager.RoleExistsAsync(role).Result) roleManager.CreateAsync(new IdentityRole(role)).Wait();
            }

            return app;
        }
    }
}