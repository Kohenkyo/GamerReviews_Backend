using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GamerReviewsApi.Repository.Helpers
{
    public class JWTGen
    {
        private readonly IConfiguration _configuration;

        public JWTGen(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string GenerateToken(int id, string usuario, int rol)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //Convierto la clave en un array de bytes con Encoding.UTF8.GetBytes
            //Paso la clave a encodear

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //le paso la llave, y uso SecurityAlgorithms.HmacSha256 para validar JWT
            //Esto asegura que el token no pueda ser manipulado


            var claims = new[]
            {
                new Claim("usuarioId", id.ToString()),
                new Claim("usuario", usuario),
                new Claim("rol", rol.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            //Convierte el JWT en un string legible
            //Este string es el token final que se enviaría al usuario.
        }
    }
}
