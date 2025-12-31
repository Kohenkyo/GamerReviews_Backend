using Dapper;
using GamerReviewsApi.EndPoints.JuegoEndP.Requests;
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GamerReviewsApi.EndPoints.JuegoEndP.Handlers
{
    public class GetPuntuacionYNumReviewsH
    {
        public async static Task<BaseResponses> GetAll(IDbConnection connection, int juego_id)
        {
            var id = juego_id;

            var resultado = await connection.QueryFirstOrDefaultAsync<GetPuntNumRevR>(
                "sp_ObtenerCalificacionJuego",
                new { JuegoId = id },
                commandType: CommandType.StoredProcedure
            );

            if (resultado == null)
            {
                return new BaseResponses(false, 404, "Los datos no se pudieron obtener");
            }

            var data = new GetPuntNumRevR
            {
                Juego_id = resultado.Juego_id,
                TotalPuntos = resultado.TotalPuntos,
                CantidadReviews = resultado.CantidadReviews,
                Calificacion = resultado.Calificacion,
            };
            

            return new DataResponse<GetPuntNumRevR>(true, 200, "Lista de juegos", data);
        }
    }
}
