using Ardalis.Specification;
using ChatVia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Application.Specifications
{
    public class GetChatWithMembersSpecifications : Specification<Chat>
    {
        public GetChatWithMembersSpecifications()
        {
            Query
                .Include(e => e.Members);
        }
    }
}
