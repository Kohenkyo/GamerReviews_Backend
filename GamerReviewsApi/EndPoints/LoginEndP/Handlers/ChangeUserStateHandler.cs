using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace GamerReviewsApi.EndPoints.LoginEndP.Handlers
{
    public class ChangeUserStateHandler
    {
        public static async Task<BaseResponses> ChangeUserState(IDbConnection connection, int id, int baja)
        {
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Usuarios SET baja = @baja WHERE usuario_id = @id",
                (SqlConnection)connection))
            {
                cmd.Parameters.AddWithValue("@baja", baja);
                cmd.Parameters.AddWithValue("@id", id);

                if (connection.State != ConnectionState.Open)
                    await ((SqlConnection)connection).OpenAsync();

                int rows = await cmd.ExecuteNonQueryAsync();

                if (rows > 0)
                    return new BaseResponses(true, (int)HttpStatusCode.OK, baja == 1 ? "Usuario dado de baja" : "Usuario reactivado");
                else
                    return new BaseResponses(false, (int)HttpStatusCode.NotFound, "Usuario no encontrado");
            }
        }
    }
}
