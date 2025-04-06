using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList;
using ToDoList.Model;

namespace ConexaoCodeFirst.Service
{
    public class TokenService
    {

        public static string  GerarToken(UserModel user)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(Configuration.PrivateKey);

            var credentials = new SigningCredentials
             (
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
             );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(3)
            };
            var token = handler.CreateToken(tokenDescriptor);

            var strToken = handler.WriteToken(token);

            return strToken;

        }

        public static ClaimsIdentity GenerateClaims(UserModel user)
        {
            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Email));
            ci.AddClaim(new Claim(ClaimTypes.Role, user.Role));

            return ci;
        }
    }
}
