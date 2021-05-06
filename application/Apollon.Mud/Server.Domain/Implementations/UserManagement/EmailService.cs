using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Apollon.Mud.Server.Domain.Interfaces.UserManagement;
using FluentEmail.Core;
using FluentEmail.Core.Defaults;
using Microsoft.Extensions.Logging;

namespace Apollon.Mud.Server.Domain.Implementations.UserManagement
{
    public class EmailService: IEmailService
    {
        /// <summary>
        /// The factory to create the emails to send.
        /// </summary>
        private readonly IFluentEmailFactory _fluentEmailFactory;

        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger, IFluentEmailFactory fluentEmailFactory)
        {
            _fluentEmailFactory = fluentEmailFactory;
            _logger = logger;
        }

        public async Task BroadcastEmail(ICollection<string> userEmails, string message, string subject)
        {
            try
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
            catch (SmtpException ex)
            {
                _logger.LogInformation(ex, "Could not send Email.");
            }
        }

        public async Task SendEmail(string userEmail, string message, string subject)
        {
            try
            {
                await _fluentEmailFactory
                    .Create()
                    .To(userEmail)
                    .Subject(subject)
                    .Body(message)
                    .SendAsync();
            }
            catch (SmtpException ex)
            {
                _logger.LogInformation(ex, "Could not send Email.");
            }
        }
    }
}