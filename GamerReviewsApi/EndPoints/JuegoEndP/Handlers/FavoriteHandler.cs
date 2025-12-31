using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using System.Data;
using Dapper;

namespace GamerReviewsApi.EndPoints.JuegoEndP.Handlers
{
    public class FavoriteHandler
    {
        public async Task<DataResponse<IsFavoriteResult>> IsFavorite(IDbConnection connection, int usuarioId, int juegoId)
        {
            try
            {
                var result = await connection.QueryFirstOrDefaultAsync<IsFavoriteResult>(
                    "sp_IsFavoriteGame",
                    new { usuario_id = usuarioId, juego_id = juegoId },
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                    return new DataResponse<IsFavoriteResult>(false, 404, "No encontrado");

                return new DataResponse<IsFavoriteResult>(true, 200, "OK", result);
            }
            catch (Exception ex)
            {
                return new DataResponse<IsFavoriteResult>(
                    false, 500, $"Error en IsFavorite: {ex.Message}"
                );
            }
        }
    }
}
