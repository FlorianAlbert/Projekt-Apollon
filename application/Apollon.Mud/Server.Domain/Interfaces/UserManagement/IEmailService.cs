using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Domain.Interfaces.UserManagement
{
    /// <summary>
    /// Service to send mails based on the configured SMTP-Server and default user.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends one email to every mail in userEmails with the same message and subject. 
        /// </summary>
        /// <param name="userEmails"></param>
        /// <param name="message"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        Task BroadcastEmail(ICollection<string> userEmails, string message, string subject);

        /// <summary>
        /// Sends one email to the userEmail with the given message and subject
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="message"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        Task SendEmail(string userEmail, string message, string subject);
    }
}