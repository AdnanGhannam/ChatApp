using Ardalis.Specification;
using ChatVia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Application.Specifications
{
    public class GetUserChatsSpecifications : Specification<Chat>
    {
        public GetUserChatsSpecifications(string userId)
        {
            Query
                .AsNoTracking()
                .Include(e => e.Messages)
                .Include(e => e.Members)
                .Where(e => e.Members.Any(m => m.Id == userId))
                .OrderBy(e => e.CreationTime);
        }
    }
}
