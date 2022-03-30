using ChatVia.Domain.Enums;
using ChatVia.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Domain.Entities
{
    // Properties
    public partial class AppUser : IdentityUser, IAggregateRoot
    {
        public string Bio { get; private set; }
        public string Location { get; private set; }
        
        [Url]
        public string ProfilePhoto { get; private set; }
        public string Status { get; private set; }
        public DateTime LastSeen { get; set; }
        public bool LastSeenVisibility { get; set; }

        public Visibilities ProfilePhotoVisibility { get; set; } = Visibilities.EveryOne;
        public Visibilities StatusVisibility { get; set; } = Visibilities.EveryOne;
        public Visibilities GroupsVisibility { get; set; } = Visibilities.EveryOne;
        public AppThemes CurrentTheme { get; set; } = AppThemes.Light;


        private readonly List<AppUser> _contacts = new();
        public IReadOnlyCollection<AppUser> Contacts => _contacts.AsReadOnly();


        private readonly List<AppUser> _savedBy = new();
        public IReadOnlyCollection<AppUser> SavedBy => _savedBy.AsReadOnly();


        private readonly HashSet<AppUser> _blockedUsers = new();
        public IReadOnlyCollection<AppUser> BlockedUsers => _blockedUsers.ToList().AsReadOnly();


        private readonly HashSet<AppUser> _blockedBy = new();
        public IReadOnlyCollection<AppUser> BlockedBy => _blockedBy.ToList().AsReadOnly();


        private readonly List<Chat> _chats = new();
        public IReadOnlyCollection<Chat> Chats => _chats.AsReadOnly();


        private readonly List<Chat> _mutedChats = new();
        public IReadOnlyCollection<Chat> MutedChats => _mutedChats.AsReadOnly();


        private readonly List<Message> _messages = new();
        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();



        // Domain Event
        [NotMapped]
        private readonly ConcurrentQueue<IDomainEvent> _domainEvents = new ConcurrentQueue<IDomainEvent>();

        [NotMapped]
        public IProducerConsumerCollection<IDomainEvent> DomainEvents => _domainEvents;
    }
}
