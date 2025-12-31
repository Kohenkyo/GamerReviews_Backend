using Dapper;
using GamerReviewsApi.EndPoints.JuegoEndP.Requests;
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GamerReviewsApi.EndPoints.JuegoEndP.Handlers
{
    public class getGames
    {
        public async Task<BaseResponses> AllGames(IDbConnection connection, HttpRequest request)
        {
            //Creamos la conexión a la base de datos y traemos los juegos

            var juegos = await connection.QueryAsync<Juego>( 
                "sp_getGames",
                commandType: CommandType.StoredProcedure
            );

            var listaJuegos = new List<Juego>(); //Lista de juegos que vamos a retornar

            foreach (var juego in juegos) //Recorremos cada juego que trajimos de la bdd
            {
                var dto = new Juego //Creamos el dto que vamos a retornar
                {
                    Id = juego.Id,
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

        public async Task<BaseResponses> AllGamesLazy(IDbConnection connection, HttpRequest request, int page, int limit)
        {
            int offset = (page - 1) * limit;

            var juegos = await connection.QueryAsync<JuegoPaginado>(
                "sp_GetGamesLazy",
                new { PageNumber = page, PageSize = limit },
                commandType: CommandType.StoredProcedure
            );

            var listaJuegos = new List<JuegoPaginado>();
            int totalPages = 0, totalRecords = 0, currentPage = page, pageSize = limit;

            foreach (var juego in juegos)
            {
                var dto = new JuegoPaginado
                {
                    Id = juego.Id,
                    Nombre = juego.Nombre,
                    Descripcion = juego.Descripcion,
                    FechaPublicacion = juego.FechaPublicacion,
                    Desarrollador = juego.Desarrollador,
                    Editor = juego.Editor,
                    Plataforma = juego.Plataforma,
                    ImagenURL = $"{request.Scheme}://{request.Host}/uploads/{juego.ImagenURL}",
                };

                listaJuegos.Add(dto);

                // Only need to read these once (they are the same for all rows)
                if (totalPages == 0)
                {
                    totalPages = juego.TotalPages;
                    totalRecords = juego.TotalRecords;
                    currentPage = juego.CurrentPage;
                    pageSize = juego.PageSize;
                }
            }

            var result = new PaginatedGamesResult
            {
                Games = listaJuegos,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                CurrentPage = currentPage,
                PageSize = pageSize
            };

            return new DataResponse<PaginatedGamesResult>(true, 200, "Lista de juegos paginada", result);
        }

        public async Task<BaseResponses> OneGame(IDbConnection connection, HttpRequest request, int game_id)
        {
            var id = game_id;

            var juegoSinTags = await connection.QueryFirstOrDefaultAsync<Juego>( //Traemos el juego con su id
                "sp_getOneGame",
                new { game_id = id },
                commandType: CommandType.StoredProcedure
            );

            if (juegoSinTags == null) //Si no existe el juego, retornamos un error 404
            {
                return new BaseResponses(false, 404, "El juego no existe");
            }

            var tags = await connection.QueryAsync<Tag>( //Traemos los tags del juego con su id
                "sp_getTagXGame",
                new { juego_id = id },
                commandType: CommandType.StoredProcedure
            );


            var data = new GetGameR
            {
                Id = juegoSinTags.Id,
                Nombre = juegoSinTags.Nombre,
                Descripcion = juegoSinTags.Descripcion,
                FechaPublicacion = juegoSinTags.FechaPublicacion?.ToString("yyyy-MM-dd"),
                Desarrollador = juegoSinTags.Desarrollador,
                Editor = juegoSinTags.Editor,
                Plataforma = juegoSinTags.Plataforma,
                ImagenURL = $"{request.Scheme}://{request.Host}/uploads/{juegoSinTags.ImagenURL}",
                Tags = tags.ToList(),
                ImagenVieja = juegoSinTags.ImagenURL
            };
            

            return new DataResponse<GetGameR>(true, 200, "Juego encontrado", data);
        }
    }
}