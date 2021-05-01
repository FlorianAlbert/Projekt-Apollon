using System.Collections.Generic;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using FluentEmail.Core;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    /// <summary>
    /// ToDo
    /// </summary>
    public class EmailService: IEmailService
    {
        /// <summary>
        /// ToDo
        /// </summary>
        private IFluentEmail _fluentEmail;

        public Task BroadcastEmail(ICollection<string> userEmails, string message, string subject)
        {
            //ToDo implement
            throw new System.NotImplementedException();
        }

        public Task SendEmail(string userEmail, string message, string subject)
        {
            //ToDo implement
            throw new System.NotImplementedException();
        }
    }
}