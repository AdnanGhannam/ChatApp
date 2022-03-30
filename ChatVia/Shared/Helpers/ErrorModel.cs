using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Shared.Helpers
{
    public class ErrorModel
    {
        public string Code { get; set; }
        public object? Description { get; set; }
        public ErrorModel(string code, object? description)
        {
            Code = code;
            Description = description;
        }
    }
}
