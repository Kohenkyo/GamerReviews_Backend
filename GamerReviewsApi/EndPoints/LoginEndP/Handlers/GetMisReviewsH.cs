using Dapper;
using GamerReviewsApi.EndPoints.LoginEndP.Requests;
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using System.Data;

namespace GamerReviewsApi.EndPoints.LoginEndP.Handlers
{
    public class GetMisReviewsH
    {
        public async static Task<BaseResponses> GetReviews(IDbConnection connection, int usuario_id)
        {
            var id = usuario_id;

            var reviews = await connection.QueryAsync<MisReviewsR>( //Traemos el juego con su id
                "sp_GetAllReviewsXUser",
                new { usuario_id = id },
                commandType: CommandType.StoredProcedure
            );

            var listaReviews = new List<MisReviewsR>(); //Lista de juegos que vamos a retornar

            foreach (var review in reviews) //Recorremos cada juego que trajimos de la bdd
            {
                var dto = new MisReviewsR //Creamos el dto que vamos a retornar
                {
                    comentario_id = review.comentario_id,
                    nombreJuego = review.nombreJuego,
                    comentario = review.comentario,
                    puntuacion = review.puntuacion,
                };

                listaReviews.Add(dto); //Agregamos el dto a la lista de juegos
            }

            return new DataResponse<IEnumerable<MisReviewsR>>(true, 200, "Lista de reviews", listaReviews); //Retornamos la lista de juegos
        }
    }
}
