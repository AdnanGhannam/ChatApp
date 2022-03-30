using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Shared.ResponseDtos
{
    public class ChatDto
    {
        public string Id { get; set; }
        public AppUserByIdDto Member { get; set; }
        public List<MessageDto> Messages { get; set; }
        public bool IsMuted { get; set; }
    }
}
