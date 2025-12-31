using Dapper; //  IMPORTANTE
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
//using GamerReviewsApi.Repository.Models;
using System.Data;

namespace GamerReviewsApi.EndPoints.ComentarioEndP.Handlers
{
    public class LikeHandler
    {
        public static async Task<DataResponse<LikeResult>> ToggleLike(IDbConnection connection, int comentarioId, int usuarioId)
        {
            try
            {
                var result = await connection.QueryFirstOrDefaultAsync<LikeResult>(
                    "sp_ToggleComentarioLike",
                    new { comentario_id = comentarioId, usuario_id = usuarioId },
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                    return new DataResponse<LikeResult>(false, 400, "No se pudo actualizar el like");

                return new DataResponse<LikeResult>(true, 200, "Like actualizado", result);
            }
            catch (Exception ex)
            {
                return new DataResponse<LikeResult>(false, 500, $"Error en ToggleLike: {ex.Message}");
            }
        }

        //  GetLike
        public static async Task<DataResponse<LikeStatus>> GetLike(IDbConnection connection, int comentarioId, int usuarioId)
        {
            try
            {
                var result = await connection.QueryFirstOrDefaultAsync<LikeStatus>(
                    "sp_GetComentarioLike",
                    new { comentario_id = comentarioId, usuario_id = usuarioId },
                    commandType: CommandType.StoredProcedure
                );

                if (result == null)
                    return new DataResponse<LikeStatus>(false, 404, "No se encontraron likes");

                return new DataResponse<LikeStatus>(true, 200, "Likes obtenidos", result);
            }
            catch (Exception ex)
            {
                return new DataResponse<LikeStatus>(false, 500, $"Error en GetLike: {ex.Message}");
            }
        }
    }
}
