using GamerReviewsApi.EndPoints.BusquedaEndP.Handlers;
using GamerReviewsApi.EndPoints.BusquedaEndP.Requests;
using GamerReviewsApi.Repository.Helpers;
using GamerReviewsApi.Repository.Interfaces;
using GamerReviewsApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GamerReviewsApi.EndPoints.BusquedaEndP
{

    [Route("api/[controller]")]
    [ApiController]
    public class BusquedaController : ControllerBase
    {
        private readonly DatabaseConecction _repo; //Inyeccion de dependencia para obtener la conexion a la dbb
        private readonly getSearchGames _getGames; //Inyeccion de dependencia para obtener los metodos del handler getGames
        public BusquedaController(DatabaseConecction repo, getSearchGames getGames)
        {
            _repo = repo;
            _getGames = getGames;
        }

        [HttpGet]
        [Route("get-search-games")]
        public async Task<BaseResponses> GetSearchGames([FromQuery] String search_term)
        {
            using var connection = _repo.CrearConexion(); //Crea la conexion a la dbb
            BaseResponses responses = await _getGames.SearchGames(connection, Request, search_term); //Llama al metodo AllGames del handler getGames
            Response.StatusCode = responses.code;
            return responses;
        }
    }
}
