using GamerReviewsApi.EndPoints.ComentarioEndP.Handlers;
using GamerReviewsApi.EndPoints.ComentarioEndP.Requests;
using GamerReviewsApi.EndPoints.JuegoEndP.Handlers;
using GamerReviewsApi.Repository.Helpers;
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace GamerReviewsApi.EndPoints.ComentarioEndPs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentarioController : ControllerBase
    {
        private readonly DatabaseConecction _repo;

        public ComentarioController(DatabaseConecction repo)
        {
            _repo = repo;
        }

        // 🔹 POST: agregar comentario
        [HttpPost("add-comentario")]
        [Authorize]
        public async Task<BaseResponses> AddComentario([FromBody] AddComentario request)
        {
            using var connection = _repo.CrearConexion();
            var response = await ComentarioHandlers.PostComentario(connection, request);
            Response.StatusCode = response.code;
            return response;
        }

        // 🔹 GET: listar comentarios por juego
        [HttpGet("get-comentarios/{juegoId}")]
        public async Task<DataResponse<IEnumerable<Comentario>>> GetComentarios(int juegoId)
        {
            using var connection = _repo.CrearConexion();
            var response = await ComentarioHandlers.GetComentariosByJuego(connection, juegoId, Request);
            Response.StatusCode = response.code;
            return response;
        }

        // 🔹 DELETE: baja lógica de comentario
        [HttpDelete("delete-comentario/{comentarioId}")]
        [Authorize]
        public async Task<BaseResponses> DeleteComentario(int comentarioId)
        {
            using var connection = _repo.CrearConexion();
            var response = await ComentarioHandlers.DeleteComentario(connection, comentarioId);
            Response.StatusCode = response.code;
            return response;
        }

        // PATCH: actualizar comentario
        [HttpPatch("update-comentario")]
        [Authorize]
        public async Task<BaseResponses> UpdateComentario([FromBody] UpdateComentario request)
        {
            using var connection = _repo.CrearConexion();
            var response = await ComentarioHandlers.UpdateComentario(connection, request);
            Response.StatusCode = response.code;
            return response;
        }

        // Like del comentario
        [HttpPost("toggle-like")]
        public async Task<DataResponse<LikeResult>> ToggleLike([FromBody] ToggleLikeR request)
        {
            using var connection = _repo.CrearConexion();
            var response = await LikeHandler.ToggleLike(connection, request.ComentarioId, request.UsuarioId);
            Response.StatusCode = response.code;
            return response;
        }
        [AllowAnonymous] //  cualquiera puede consultar, porque no neceaita el jwt del usuario,osea no necesita estar loguedo para ver los likes, ok?
        [HttpGet("get-like")]
        public async Task<DataResponse<LikeStatus>> GetLike([FromQuery] int comentarioId, [FromQuery] int usuarioId)
        {
            using var connection = _repo.CrearConexion();
            var response = await LikeHandler.GetLike(connection, comentarioId, usuarioId);
            Response.StatusCode = response.code;
            return response;
        }

    }
}
