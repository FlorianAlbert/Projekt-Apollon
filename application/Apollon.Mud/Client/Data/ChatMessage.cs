using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apollon.Mud.Client.Data
{
    public class ChatMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RecipientName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool MessageIncoming { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MessageText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="text"></param>
        /// <param name="incoming"></param>
        /// <param name="stamp"></param>
        public ChatMessage(string sender, string recipient, string text, bool incoming)
        {
            SenderName = sender;
            RecipientName = recipient;
            MessageText = text;
            MessageIncoming = incoming;
            Timestamp = DateTime.Now;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="stamp"></param>
        public ChatMessage(string name, string text, bool incoming)
        {
            MessageIncoming = incoming;
            if (incoming) SenderName = name;
            else RecipientName = name;
            MessageText = text;
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg1"></param>
        /// <param name="msg2"></param>
        /// <returns></returns>
        public static bool operator <(ChatMessage msg1, ChatMessage msg2)
        {
            return msg1.Timestamp < msg2.Timestamp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg1"></param>
        /// <param name="msg2"></param>
        /// <returns></returns>
        public static bool operator >(ChatMessage msg1, ChatMessage msg2)
        {
            return msg1.Timestamp > msg2.Timestamp;
        }

    }
}
