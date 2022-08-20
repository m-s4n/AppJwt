using AppJwt.Core.Dtos;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppJwt.Core.Services
{
    public interface IAuthenticationService
    {
        // Login
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        // Refresh Token ile login
        Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken);
        // Refresh token Yoketme
        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);
        // Client Login
        Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    }
}
