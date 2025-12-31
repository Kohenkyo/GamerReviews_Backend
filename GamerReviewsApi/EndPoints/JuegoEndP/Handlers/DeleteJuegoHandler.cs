using GamerReviewsApi.EndPoints.JuegoEndP.Requests;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace GamerReviewsApi.EndPoints.JuegoEndP.Handlers
{
    public class DeleteJuegoHandler
    {
        public async Task<BaseResponses> DarDeBajaJuego(IDbConnection connection, DeleteJuegoR request)
        {
            using (SqlCommand cmd = new SqlCommand("SP_Update_Juego_Baja", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@JuegoId", request.Id);

                if (connection.State != ConnectionState.Open)
                    await ((SqlConnection)connection).OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        bool success = reader.GetInt32(0) == 1;
                        string message = reader.GetString(1);

                         return new BaseResponses(success, success ? 200 : 404, message);
                       
                    }
                }
            }

            return new BaseResponses(false, 500, "Error al dar de baja el juego");
        }
    }
}
