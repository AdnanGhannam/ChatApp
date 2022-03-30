using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Domain.Entities
{
    // Methods
    public partial class AppUser
    {
        private AppUser() { }

        public AppUser(string username,
            string email,
            string bio = "",
            string location = "")
        {
            UserName = username;
            Email = email;
            Bio = bio;
            Location = location;
            LastSeenVisibility = true;
            Status = "Hi, I am new here";
            ProfilePhoto = "/";
        }

        public void MuteChat(Chat chat)
        {
            if (!_mutedChats.Any(c => c.Id == chat.Id))
            {
                _mutedChats.Add(chat);
            }
        }

        public void UnmuteChat(Chat chat)
        {
            _mutedChats.Remove(chat);
        }

        public void AddContact(AppUser contact)
        {
            _contacts.Add(contact);
        }

        public void RemoveContact(AppUser contact)
        {
            _contacts.Remove(contact);
        }
    }
}
