using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AppJwt.API.Helpers
{
    public class TokenOperator
    {
        public static object DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            string pureToken = token.Replace("Bearer ", string.Empty);
            var jwtSecurityToken = handler.ReadJwtToken(pureToken);

            var result = new
            {
                Name = jwtSecurityToken.Claims.First(c => c.Type == "user_name").Value,
                Audiences = jwtSecurityToken.Claims.Where(c => c.Type == "aud").Select(c => c.Value).ToArray(),
            };

            return result;
        }
    }
}
