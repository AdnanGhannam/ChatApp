using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Domain.Entities
{
    // Methods
    public partial class Chat
    {
        private Chat() { }

        public Chat(AppUser user1,
            AppUser user2)
        {
            if(user1 == null || user2 == null)
            {
                throw new ArgumentNullException("Members can't be null");
            }
            
            _members.Add(user1);
            _members.Add(user2);
        }

        public void SendMessage(Message message)
        {
            if(message is not null && message is { Chat: not null, Sender: not null, Content: not null })
            {
                _messages.Add(message);
                return;
            }

            throw new ArgumentNullException();
        }

        public void SendMessage(string senderId, string messageContent)
        {
            if(senderId == null)
            {
                throw new ArgumentNullException("Sender-Id can't be null");
            }

            if(messageContent == null)
            {
                throw new ArgumentNullException("Message-Content can't be null");
            }

            var message = new Message(senderId, this.Id, messageContent);
            _messages.Add(message);
        }
    }
}
