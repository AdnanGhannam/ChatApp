using Ardalis.Specification;
using ChatVia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Application.Specifications
{
    public class GetChatByMembersSpecifications : Specification<Chat>
    {
        public GetChatByMembersSpecifications(Expression<Func<Chat, bool>> criteria)
        {
            Query
                .Include(c => c.Members)
                .Include(c => c.Messages)
                .Where(criteria);
        }
    }
}
