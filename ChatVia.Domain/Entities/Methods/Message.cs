using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Domain.Entities
{
    // Methods
    public partial class Message
    {
        private Message() { }

        public Message(string senderId,
            string chatId,
            string content)
        {
            SenderId = senderId;
            ChatId = chatId;
            Content = content;
        }

        public Message(AppUser sender,
            Chat chat,
            string content)
        {
            Sender = sender;
            Chat = chat;
            Content = content;
        }
    }
}
