using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Shared.ResponseDtos
{
    public class MessageDto
    {
        public string Content { get; set; }

        public string SenderId { get; set; }
        public AppUserByIdDto Sender { get; set; }

        public string ChatId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
