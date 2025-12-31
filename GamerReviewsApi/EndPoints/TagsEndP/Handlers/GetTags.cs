using GamerReviewsApi.Responses;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.EndPoints.TagsEndP.Requests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GamerReviewsApi.EndPoints.TagsEndP.Handlers
{
    public class GetTags
    {
        public async static Task<BaseResponses> AllTags(IDbConnection connection)
        {
            var querytags = await connection.QueryAsync<GetTagsR>(
               "sp_GetAllTags",
               commandType: CommandType.StoredProcedure
           );

            var listaTags = new List<GetTagsR>(); //Lista de juegos que vamos a retornar

            foreach (var tags in querytags) //Recorremos cada juego que trajimos de la bdd
            {
                var dto = new GetTagsR //Creamos el dto que vamos a retornar
                {
                    tag_id = tags.tag_id,
                    nombre = tags.nombre
                };

                listaTags.Add(dto); //Agregamos el dto a la lista de juegos
            }

            return new DataResponse<IEnumerable<GetTagsR>>(true, 200, "Lista de tags", listaTags); //Retornamos la lista de juegos
        }

        public async static Task<BaseResponses> TagsXgame(IDbConnection connection, int juego_id)
        {
            var id = juego_id;

            var querytags = await connection.QueryAsync<GetTagsR>(
               "sp_GetTagXGame",
               new { juego_id = id },
            commandType: CommandType.StoredProcedure
           );

            var listaTags = new List<GetTagsR>(); //Lista de juegos que vamos a retornar

            foreach (var tags in querytags) //Recorremos cada juego que trajimos de la bdd
            {
                var dto = new GetTagsR //Creamos el dto que vamos a retornar
                {
                    tag_id = tags.tag_id,
                    nombre = tags.nombre
                };

                listaTags.Add(dto); //Agregamos el dto a la lista de juegos
            }

            return new DataResponse<IEnumerable<GetTagsR>>(true, 200, "Lista de tags", listaTags); //Retornamos la lista de juegos
        }
    }
}
