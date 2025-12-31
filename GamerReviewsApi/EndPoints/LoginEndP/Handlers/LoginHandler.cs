using Dapper;
using GamerReviewsApi.Repository.Helpers;
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using System.Data;

namespace GamerReviewsApi.EndPoints.LoginEndP.Handlers
{
    public class LoginHandler
    {
        public static async Task<BaseResponses> DoLogin(IDbConnection connection, string correo, string contrasena, JWTGen jwtgen)
        {
            try
            {
                correo = correo.Trim().ToLower();

                var query = @"SELECT usuario_id AS usuarioId, nombre AS usuario, rol
                              FROM Usuarios
                              WHERE correo = @correo AND contrasena = @contrasena AND baja = 0";

                var user = await connection.QueryFirstOrDefaultAsync<LoginUser>(
                    query,
                    new { correo,  contrasena }
                );

                if (user == null)
                {
                    return new BaseResponses(false, 401, "Usuario o contraseña incorrectos.");
                }

                var token = jwtgen.GenerateToken(user.usuarioId, user.usuario, user.rol);

                return new DataResponse<object>(true, 200, "Login exitoso.", token);
            }
            catch (Exception ex)
            {
                return new BaseResponses(false, 500, $"Error en login: {ex.Message}");
            }
        }
    }
}
