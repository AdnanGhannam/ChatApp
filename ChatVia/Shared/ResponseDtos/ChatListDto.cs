using ChatVia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Shared.ResponseDtos
{
    public class ChatListDto
    {
        public string Id { get; set; }
        public AppUserByIdDto Member { get; set; }
        public MessageDto Message { get; set; }
        public bool IsActive { get; set; }
    }
}
