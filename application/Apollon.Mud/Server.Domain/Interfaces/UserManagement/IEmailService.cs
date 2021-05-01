using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apollon.Mud.Server.Domain.Interfaces.UserManagement
{
    /// <summary>
    /// ToDo
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userEmails"></param>
        /// <param name="message"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        Task BroadcastEmail(ICollection<string> userEmails, string message, string subject); //ToDo in UML abändern

        /// <summary>
        /// ToDo
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="message"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        Task SendEmail(string userEmail, string message, string subject); //ToDo in UML abändern
    }
}