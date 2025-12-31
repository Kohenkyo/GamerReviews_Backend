using Dapper;
using GamerReviewsApi.EndPoints.JuegoEndP.Requests;
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace GamerReviewsApi.EndPoints.JuegoEndP.Handlers
{
    public class getMyGames
    {
        public async Task<BaseResponses> AllMyGames(IDbConnection connection, int usuarioId, HttpRequest request)
        {
            var id = usuarioId;

            var juegos = await connection.QueryAsync<Juego>(
                "sp_GetMyGames",
                new {usuario_id = id},
                commandType: CommandType.StoredProcedure
            );

            var listaJuegos = new List<Juego>(); //Lista de juegos que vamos a retornar

            foreach (var juego in juegos) //Recorremos cada juego que trajimos de la bdd
            {
                var dto = new Juego //Creamos el dto que vamos a retornar
                {
                    Id = (int)juego.Id,
                    Nombre = juego.Nombre,
                    Descripcion = juego.Descripcion,
                    FechaPublicacion = juego.FechaPublicacion,
                    Desarrollador = juego.Desarrollador,
                    Editor = juego.Editor,
                    Plataforma = juego.Plataforma,
                    ImagenURL = $"{request.Scheme}://{request.Host}/uploads/{juego.ImagenURL}",
                };

                listaJuegos.Add(dto); //Agregamos el dto a la lista de juegos
            }

            return new DataResponse<IEnumerable<Juego>>(true, 200, "Lista de juegos", listaJuegos); //Retornamos la lista de juegos


        }
    }
}
