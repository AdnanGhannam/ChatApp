using ChatVia.Domain.Base;
using ChatVia.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Domain.Entities
{
    // Properties
    public partial class Chat : EntityBase, IAggregateRoot
    {
        private readonly List<AppUser> _members = new();
        public IReadOnlyCollection<AppUser> Members => _members.AsReadOnly();


        private readonly List<Message> _messages = new();
        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();


        private readonly List<AppUser> _mutedBy = new();
        public IReadOnlyCollection<AppUser> MutedBy => _mutedBy.AsReadOnly();
    }
}
