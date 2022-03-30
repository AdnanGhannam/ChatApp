using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Shared.ResponseDtos
{
    public class AppUserByIdDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public string ProfilePhoto { get; set; }
        public string Status { get; set; }
        public DateTime LastSeen { get; set; }

        public List<AppUserByIdDto> Contacts { get; set; }
        public List<AppUserByIdDto> BlockedUsers { get; set; }
    }
}
