using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;

namespace GamerReviewsApi.EndPoints.TagsEndP.Handlers
{
    public class DeleteTag
    {
        public async static Task<BaseResponses> Delete(IDbConnection connection, int tag_id)
        {
            using (SqlCommand cmd = new SqlCommand("sp_DeleteTag", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tag_id", tag_id);

                if (connection.State != ConnectionState.Open) // Abre la conexión si no está ya abierta
                    await ((SqlConnection)connection).OpenAsync();
          
                await cmd.ExecuteNonQueryAsync();

                return new BaseResponses(true, 200, "Tag eliminada");
            }
        }

        public async static Task<BaseResponses> DeleteTagXGame(IDbConnection connection, int juego_id, int tag_id)
        {
            using (SqlCommand cmd = new SqlCommand("sp_DeleteTagXGame", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@juego_id", juego_id);
                cmd.Parameters.AddWithValue("@tag_id", tag_id);

                if (connection.State != ConnectionState.Open) // Abre la conexión si no está ya abierta
                    await ((SqlConnection)connection).OpenAsync();

                await cmd.ExecuteNonQueryAsync();

                return new BaseResponses(true, 200, "Tag eliminada");
            }
        }
    }
}
