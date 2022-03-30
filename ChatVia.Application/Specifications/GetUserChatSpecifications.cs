using Ardalis.Specification;
using ChatVia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Application.Specifications
{
    public class GetUserChatSpecifications : Specification<Chat>
    {
        public GetUserChatSpecifications()
        {
            Query
                .Include(e => e.Members)
                .Include(e => e.Messages)
                .Include(e => e.MutedBy);
        }
    }
}
