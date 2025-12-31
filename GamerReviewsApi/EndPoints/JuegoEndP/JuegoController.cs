using GamerReviewsApi.EndPoints.JuegoEndP.Handlers;
using GamerReviewsApi.EndPoints.JuegoEndP.Requests;
using GamerReviewsApi.Repository.Helpers;
using GamerReviewsApi.Repository.Interfaces;
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GamerReviewsApi.EndPoints.JuegoEndP
{
    [ApiController]
    [Route("api/[controller]")]
    public class JuegoController : Controller
    {
        private readonly DatabaseConecction _repo; //Inyeccion de dependencia para obtener la conexion a la dbb
        protected readonly postGame _postGame; //Inyeccion de dependencia para obtener los metodos del handler postGame
        protected readonly getGames _getGames; //Inyeccion de dependencia para obtener los metodos del handler getGames
        protected readonly postFavoriteGame _postFavoriteGame; //Inyeccion de dependencia para obtener los metodos del handler postFavoriteGame
        private readonly postProxJuego _postProxJuego;
        protected readonly IFileStorageService _fileStorage; //intyecta
        protected readonly getProxJuegos _getProxJuegos;
        private readonly getMyGames _getMyGames;// obtener mis favoritos
        private readonly FavoriteHandler _favoriteHandler;
        private readonly DeleteProxJuegoHandler _deleteProxJuegoHandler;
        private readonly DeleteJuegoHandler _deleteJuegoHandler;


        public JuegoController(DatabaseConecction repo, postGame postGame, getGames getGames, postFavoriteGame postFavoriteGame,
                               IFileStorageService fileStorage, FavoriteHandler favoriteHandler, 
                               DeleteProxJuegoHandler deleteProxJuegoHandler, DeleteJuegoHandler deleteJuegoHandler)
        {
            _repo = repo;
            _postGame = postGame;
            _getGames = getGames;
            _postFavoriteGame = postFavoriteGame;
            _fileStorage = fileStorage;
            _postProxJuego = new postProxJuego(_fileStorage);
            _getProxJuegos = new getProxJuegos();
            _getMyGames = new getMyGames(); //inicializa el handler
            _favoriteHandler = favoriteHandler;
            _deleteProxJuegoHandler = deleteProxJuegoHandler;
            _deleteJuegoHandler = deleteJuegoHandler;
        }

        [HttpPost]
        [Route("create-juego")]
        [Authorize]
        public async Task<BaseResponses> CreateJuego([FromForm] AddGameR request) //Creo un nuevo juego en la bdd
        {
            using var connection = _repo.CrearConexion(); //Crea la conexion a la dbb
            BaseResponses responses = await _postGame.PostOneGame(connection, request.Nombre, request.Descripcion, (DateTime)request.FechaCreacion, request.Desarrollador, request.Editor, request.Plataforma, request.Imagen); //Llama al metodo PostOneGame del handler postGame
            Response.StatusCode = responses.code;
            return responses;
        }

        [HttpGet]
        [Route("get-all-games")]
        public async Task<BaseResponses> GetAllGames() //Llama a todos los juegos de la bdd
        {
            using var connection = _repo.CrearConexion(); //Crea la conexion a la dbb
            BaseResponses responses = await _getGames.AllGames(connection,Request); //Llama al metodo AllGames del handler getGames
            Response.StatusCode = responses.code;
            return responses;
        }

        [HttpGet]
        [Route("get-one-game")]
        public async Task<BaseResponses> GetOneGame(int game_id) //Llama a un juego de la bdd por su id
        {
            using var connection = _repo.CrearConexion(); //Crea la conexion a la dbb
            BaseResponses responses = await _getGames.OneGame(connection, Request, game_id); //Llama al metodo OneGame del handler getGames
            Response.StatusCode = responses.code;
            return responses;
        }

        [HttpGet]
        [Route("get-all-games-lazy")]
        public async Task<BaseResponses> GetAllGamesLazy(int page, int limit) // Llama a los juegos de la bdd con paginacion
        {
            using var connection = _repo.CrearConexion();
            BaseResponses responses = await _getGames.AllGamesLazy(connection, Request, page, limit); // Llama al metodo AllGamesLazy del handler getGames
            Response.StatusCode = responses.code;
            return responses;
        }

        [HttpPost]
        [Route("add-favorite-game")]
        [Authorize]
        public async Task<BaseResponses> AddFavGame([FromBody] FavoriteGame request)
        {   
            using var connection = _repo.CrearConexion(); //Crea la conexion a la dbb
            BaseResponses responses = await _postFavoriteGame.PostOneFavoriteGame(connection, request.JuegoId, request.UsuarioId, request.BotonCheck); //Llama al metodo AddFavGame del handler getGames
            Response.StatusCode = responses.code;
            return responses;
        }

        //Proximos lanzamiento
        [HttpPost]
        [Route("create-proxjuego")]
        [Authorize]
        public async Task<BaseResponses> CreateProxJuego([FromForm] AddProxJuegoR request)
        {
            using var connection = _repo.CrearConexion();

            BaseResponses responses = await _postProxJuego.PostOneProxJuego(
                connection,
                request.Nombre,
                request.Imagen
            );

            Response.StatusCode = responses.code;
            return responses;
        }

        [HttpGet]
        [Route("get-proxjuegos")]
        public async Task<BaseResponses> GetAllProxJuegos()
        {
            using var connection = _repo.CrearConexion();
            BaseResponses responses = await _getProxJuegos.AllProxJuegos(connection, Request);
            Response.StatusCode = responses.code;
            return responses;
        }

        [HttpDelete]
        [Route("delete-proxjuego")]
        [Authorize]
        public async Task<BaseResponses> DeleteProxJuego([FromQuery] int id, string imagenVieja)
        {
            using var connection = _repo.CrearConexion();
            BaseResponses responses = await _deleteProxJuegoHandler.DeleteProxJuego(connection, id, imagenVieja);
            Response.StatusCode = responses.code;
            return responses;
        }

        //  GET: Ya tengo add-favorite-game expuesto en tu controller como add-favorite-game — ese POST llamará a postFavoriteGame
        [HttpGet]
        [Route("get-my-games/{usuarioId}")]
        public async Task<BaseResponses> GetMyGames(int usuarioId)
        {
            using var connection = _repo.CrearConexion();
            var responses = await _getMyGames.AllMyGames(connection, usuarioId, Request);
            Response.StatusCode = responses.code;
            return responses;
        }

        [HttpGet()]
        [Route("is-favorite")]
        public async Task<BaseResponses> IsFavorite([FromQuery] int usuarioId, [FromQuery] int juegoId)
        {
            using var connection = _repo.CrearConexion();
            var response = await _favoriteHandler.IsFavorite(connection, usuarioId, juegoId);
            Response.StatusCode = response.code;
            return response;
        }

        [HttpPatch]
        [Route("edit-game")]
        [Authorize]
        public async Task<BaseResponses> EditGame([FromForm] UpdateGameR request, [FromServices] UpdateGameHandler handler)
        {
            using var connection = _repo.CrearConexion();
            var response = await handler.EditGameById(
                connection,
                request.JuegoId,
                request.Nombre,
                request.Descripcion,
                request.FechaPublicacion,
                request.Desarrollador,
                request.Editor,
                request.Plataforma,
                request.Imagen,
                request.ImagenVieja
            );

            Response.StatusCode = response.code;
            return response;
        }

        //eliminar Juego -> baja=1
        [HttpDelete("delete-juego")]
        [Authorize]
        public async Task<BaseResponses> DarDeBajaJuego([FromQuery] int id)
        {
            var connection = _repo.CrearConexion();
            var result = await _deleteJuegoHandler.DarDeBajaJuego(connection, new DeleteJuegoR { Id = id });
            Response.StatusCode = result.code;
            return result;
        }

        [HttpGet("getPuntuacion/NumReviews")]
        public async Task<BaseResponses> GetPuntuacionYNumReviews([FromQuery] int juego_id)
        {
            var connection = _repo.CrearConexion();
            var result = await GetPuntuacionYNumReviewsH.GetAll(connection, juego_id);
            Response.StatusCode = result.code;
            return result;
        }

        [HttpGet]
        [Route("get-ranking")]
        public async Task<BaseResponses> GetRanking()
        {
            using var connection = _repo.CrearConexion();
            var responses = await GetRankingH.GetTopRanking(connection,Request);
            Response.StatusCode = responses.code;
            return responses;
        }
    }
}
