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
    /// <inheritdoc cref="IEmailService"/>
    public class EmailService: IEmailService
    {
        #region member
        /// <summary>
        /// The factory to create the emails to send.
        /// </summary>
        private readonly IFluentEmailFactory _fluentEmailFactory;
        #endregion


        public EmailService(IFluentEmailFactory fluentEmailFactory)
        {
            _fluentEmailFactory = fluentEmailFactory;
        }

        #region methods
        /// <inheritdoc cref="IEmailService.BroadcastEmail"/>
        public async Task BroadcastEmail(ICollection<string> userEmails, string message, string subject)
        {
            if (userEmails is null || message is null || subject is null) return;
            foreach (var mail in userEmails)
            {
                if(mail is null) continue;
                await _fluentEmailFactory
                    .Create()
                    .To(mail)
                    .Subject(subject)
                    .Body(message)
                    .SendAsync();
            }

        }

        /// <inheritdoc cref="IEmailService.SendEmail"/>
        public async Task SendEmail(string userEmail, string message, string subject)
        {
            if (userEmail is null || message is null || subject is null) return;
            await _fluentEmailFactory
                .Create()
                .To(userEmail)
                .Subject(subject)
                .Body(message)
                .SendAsync();
        }
        #endregion

    }
}