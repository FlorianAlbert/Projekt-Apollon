using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using FluentEmail.Core;
using FluentEmail.Core.Defaults;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    public class EmailService: IEmailService
    {
        /// <summary>
        /// The factory to create the emails to send.
        /// </summary>
        private readonly IFluentEmailFactory _fluentEmailFactory;

        public EmailService(IFluentEmailFactory fluentEmailFactory)
        {
            _fluentEmailFactory = fluentEmailFactory;
        }

        public async Task BroadcastEmail(ICollection<string> userEmails, string message, string subject)
        {
            foreach (var mail in userEmails)
            {
                await _fluentEmailFactory
                    .Create()
                    .To(mail)
                    .Subject(subject)
                    .Body(message)
                    .SendAsync();
            }
           
        }

        public async Task SendEmail(string userEmail, string message, string subject)
        {
            await _fluentEmailFactory
                .Create()
                .To(userEmail)
                .Subject(subject)
                .Body(message)
                .SendAsync();
        }
    }
}