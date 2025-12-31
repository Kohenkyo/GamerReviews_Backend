using GamerReviewsApi.EndPoints.TagsEndP.Requests;
using GamerReviewsApi.EndPoints.TagsEndP.Handlers;
using GamerReviewsApi.Repository.Helpers;
using GamerReviewsApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GamerReviewsApi.EndPoints.TagsEndP
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly DatabaseConecction _repo; //Inyeccion de dependencia para obtener la conexion a la dbb

        public TagController(DatabaseConecction repo)
        {
            _repo = repo;
        }

        [HttpPost("create-tag")]
        [Authorize]
        public async Task<BaseResponses> AddNewTag([FromBody] AddTagR request)
        {
            using var connection = _repo.CrearConexion(); //Crear la conexion a la dbb
            var response = await AddTag.AddOneTag(connection, request.nombre);
            Response.StatusCode = response.code;
            return response;
        }

        [HttpGet("get-tags")]
        public async Task<BaseResponses> GetAllTags()
        {
            using var connection = _repo.CrearConexion(); //Crear la conexion a la dbb
            var response = await GetTags.AllTags(connection);
            Response.StatusCode = response.code;
            return response;
        }

        [HttpDelete("delete-tag")]
        [Authorize]
        public async Task<BaseResponses> DeleteOneTag([FromQuery] int tag_id)
        {
            using var connection = _repo.CrearConexion(); //Crear la conexion a la dbb
            var response = await DeleteTag.Delete(connection, tag_id);
            Response.StatusCode = response.code;
            return response;
        }

        [HttpPost("insert-tagXgame")]
        [Authorize]
        public async Task<BaseResponses> InsertTagXGame([FromBody] TagXGameR request)
        {
            using var connection = _repo.CrearConexion(); //Crear la conexion a la dbb
            var response = await AddTag.TagXgame(connection, request.juego_id, request.tag_id);
            Response.StatusCode = response.code;
            return response;
        }

        [HttpGet("get-tagsXgame")]
        public async Task<BaseResponses> GetTagsXGame([FromQuery] int juego_id)
        {
            using var connection = _repo.CrearConexion(); //Crear la conexion a la dbb
            var response = await GetTags.TagsXgame(connection, juego_id);
            Response.StatusCode = response.code;
            return response;
        }

        [HttpDelete("delete-tagXgame")]
        [Authorize]
        public async Task<BaseResponses> DeleteTagXGame([FromQuery] int juego_id, int tag_id)
        {
            using var connection = _repo.CrearConexion(); //Crear la conexion a la dbb
            var response = await DeleteTag.DeleteTagXGame(connection, juego_id, tag_id);
            Response.StatusCode = response.code;
            return response;
        }
    }
}
