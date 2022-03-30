using ChatVia.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Domain.Entities
{
    // Properties
    public partial class Message : EntityBase
    {
        public string Content { get; private set; }


        public string SenderId { get; private set; }
        public AppUser Sender { get; private set; }

        public string ChatId { get; private set; }
        public Chat Chat { get; private set; }
    }
}
