using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace GamerReviewsApi.EndPoints.LoginEndP.Handlers
{
    public class GetAllUsersHandler
    {
        public static async Task<DataResponse<List<LoginUser>>> GetAllUsers(IDbConnection connection, HttpRequest request)
        {
            var lista = new List<LoginUser>();

            using (SqlCommand cmd = new SqlCommand(
                "SELECT usuario_id, correo, nombre, perfilURL, rol, baja FROM Usuarios",
                (SqlConnection)connection))
            {
                if (connection.State != ConnectionState.Open)
                    await ((SqlConnection)connection).OpenAsync();

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        string? fotoArchivo = reader.IsDBNull(3) ? null : reader.GetString(3);

                        lista.Add(new LoginUser
                        {
                            usuarioId = reader.GetInt32(0),
                            correo = reader.GetString(1),
                            usuario = reader.GetString(2), // nombre
                            perfilURL = string.IsNullOrEmpty(fotoArchivo)
                                ? null
                                : $"{request.Scheme}://{request.Host}/users/{fotoArchivo}",
                            rol = reader.GetByte(4),
                            baja = reader.GetByte(5)
                        });
                    }
                }
            }

            return new DataResponse<List<LoginUser>>(true, (int)HttpStatusCode.OK, "Usuarios obtenidos con éxito", lista);
        }
    }
}
