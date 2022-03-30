using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Shared.RequestDtos
{
    public record AppUserRegisterDto(
        string UserName, string Email, string Bio, string Location, string Password);
}
