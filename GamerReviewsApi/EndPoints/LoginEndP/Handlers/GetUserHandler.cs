using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;
using System.Net;

public class GetUserHandler
{
    public static async Task<DataResponse<LoginUser>> GetOneUser(IDbConnection connection, int usuarioId, HttpRequest request)
    {
        using (SqlCommand cmd = new SqlCommand(
            "SELECT correo, nombre, perfilURL, rol, baja FROM Usuarios WHERE usuario_id = @usuario_id",
            (SqlConnection)connection))
        {
            cmd.Parameters.AddWithValue("@usuario_id", usuarioId);

            if (connection.State != ConnectionState.Open)
                await ((SqlConnection)connection).OpenAsync();

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    // PerfilURL puede ser null
                    string? perfilUrlRaw = reader.IsDBNull(2) ? null : reader.GetString(2);

                    var user = new LoginUser
                    {
                        correo = reader.GetString(0),
                        usuario = reader.GetString(1),
                        perfilURL = reader.IsDBNull(2) ? null : $"{request.Scheme}://{request.Host}/users/{perfilUrlRaw}",
                        rol = reader.GetByte(3),
                        baja = reader.GetByte(4),
                        foto = perfilUrlRaw
                    };


                    return new DataResponse<LoginUser>(true, (int)HttpStatusCode.OK, "Usuario encontrado", user);
                }
                else
                {
                    return new DataResponse<LoginUser>(false, (int)HttpStatusCode.NotFound, "Usuario no encontrado");
                }
            }
        }
    }
}
