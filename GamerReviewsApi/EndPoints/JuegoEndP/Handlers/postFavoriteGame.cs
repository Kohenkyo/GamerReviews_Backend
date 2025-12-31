using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GamerReviewsApi.EndPoints.JuegoEndP.Handlers
{
    public class postFavoriteGame
    {
        public async Task<BaseResponses> PostOneFavoriteGame(IDbConnection connection, int juegoId, int usuarioId, bool botonCheck)
        {
            try
            {
                using (var cmd = new SqlCommand("sp_AddOrRemoveFavorite", (SqlConnection)connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@usuario_id", usuarioId);
                    cmd.Parameters.AddWithValue("@juego_id", juegoId);
                    cmd.Parameters.AddWithValue("@botonCheck", botonCheck ? 1 : 0); // BIT

                    if (connection.State != ConnectionState.Open)
                        await ((SqlConnection)connection).OpenAsync();

                    await cmd.ExecuteNonQueryAsync();

                    return new BaseResponses(true, (int)HttpStatusCode.OK, "Operación de favorito ejecutada correctamente.");
                }
            }
            catch (Exception ex)
            {
                return new BaseResponses(false, (int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
