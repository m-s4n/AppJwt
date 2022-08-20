using AppJwt.Core.Configurations;
using AppJwt.Core.Dtos;
using AppJwt.Core.Entities;
using AppJwt.Core.Repositories;
using AppJwt.Core.Services;
using AppJwt.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;


namespace AppJwt.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _client;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserRefreshToken> _userResreshTokenRepository;

        public AuthenticationService(
            IOptions<List<Client>> optionsClient,
            ITokenService tokenService,
            UserManager<User> userManager,
            IUnitOfWork unitOfWork,
            IRepository<UserRefreshToken> userResreshTokenRepository)
        {
            _client = optionsClient.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userResreshTokenRepository = userResreshTokenRepository;
        }

        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Response<TokenDto>.Fail("Email veya şifre yanlış", 400, true);

            if (!(await _userManager.CheckPasswordAsync(user, loginDto.Password)))
            {
                return Response<TokenDto>.Fail("Email veya şifre yanlış", 400, true);
            }

            var token = _tokenService.CreateToken(user);
            var userRefreshToken = await _userResreshTokenRepository.Where(u => u.UserId == user.Id).SingleOrDefaultAsync();
            // refresh token yok ise kayıt edilir.
            // var ise refresh token yenilenir
            if (userRefreshToken == null)
            {
                await _userResreshTokenRepository.AddAsync(new UserRefreshToken() { UserId = user.Id, Token = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.Token = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(token, 200);


        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _client.SingleOrDefault(c => c.Id == clientLoginDto.ClientId && c.Secret == clientLoginDto.ClientSecret);

            // client null ise
            if (client == null)
            {
                return Response<ClientTokenDto>.Fail("Client veya secret bulunamdı", 404, true);
            }

            var token = _tokenService.CreateClientToken(client);

            return Response<ClientTokenDto>.Success(token, 200);
        }

        // refresh token ile token alma
        public async Task<Response<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            var currrentRefreshToken = await _userResreshTokenRepository.Where(x => x.Token == refreshToken).SingleOrDefaultAsync();

            if (currrentRefreshToken == null)
            {
                return Response<TokenDto>.Fail("RefreshToken not found", 404, true);
            }

            var user = await _userManager.FindByIdAsync(currrentRefreshToken.UserId);

            if (user == null)
            {
                return Response<TokenDto>.Fail("UserId not found", 404, true);
            }

            var tokenDto = _tokenService.CreateToken(user);

            currrentRefreshToken.Token = tokenDto.RefreshToken;
            currrentRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return Response<TokenDto>.Success(tokenDto, 200);



        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var rToken = await _userResreshTokenRepository.Where(u => u.Token == refreshToken).SingleOrDefaultAsync();
            if (rToken == null)
            {
                return Response<NoDataDto>.Fail("refresh token not found", 404, true);
            }

            _userResreshTokenRepository.Remove(rToken);

            await _unitOfWork.CommitAsync();

            return Response<NoDataDto>.Success(200);
        }
    }
}
