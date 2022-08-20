using AppJwt.Core.Configurations;
using AppJwt.Core.Dtos;
using AppJwt.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppJwt.Core.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(User user);
        ClientTokenDto CreateClientToken(Client client);
    }
}
