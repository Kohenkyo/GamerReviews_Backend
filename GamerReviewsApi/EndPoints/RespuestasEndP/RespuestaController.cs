using GamerReviewsApi.EndPoints.RespuestasEndP.Handlers;
using GamerReviewsApi.EndPoints.RespuestasEndP.Requests;
using GamerReviewsApi.Repository.Helpers;
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamerReviewsApi.EndPoints.RespuestasEndP
{
    [Route("api/[controller]")]
    [ApiController]
    public class RespuestaController : ControllerBase
    {
        private readonly DatabaseConecction _repo; //Inyeccion de dependencia para obtener la conexion a la dbb

        public RespuestaController(DatabaseConecction repo)
        {
            _repo = repo;
        }

        // POST: agregar respuesta
        [HttpPost("add-respuesta")]
        [Authorize]
        public async Task<BaseResponses> AddRespuesta([FromBody] AddRespuesta request)
        {
            using var connection = _repo.CrearConexion(); //Crear la conexion a la dbb
            var response = await RespuestaHandlers.PostRespuesta(connection,request);
            Response.StatusCode = response.code;
            return response;
        }

        // GET: listar respuestas de un comentario
        [HttpGet("get-respuestas/{comentarioId}")]
        public async Task<DataResponse<IEnumerable<Respuesta>>> GetRespuestas(int comentarioId)
        {
            using var connection = _repo.CrearConexion(); //Crear la conexion a la dbb
            var response = await RespuestaHandlers.GetRespuestasByComentario(connection, comentarioId, Request);
            Response.StatusCode = response.code;
            return response;
        }

        // DELETE: baja lógica de respuesta
        [HttpDelete("delete-respuesta/{respuestaId}")]
        [Authorize]
        public async Task<BaseResponses> DeleteRespuesta(int respuestaId)
        {
            using var connection = _repo.CrearConexion(); //Crear la conexion a la dbb
            var response = await RespuestaHandlers.DeleteRespuesta(connection, respuestaId);
            Response.StatusCode = response.code;
            return response;
        }

        // PATCH: actualizar respuesta
        [HttpPatch("update-respuesta")]
        [Authorize]
        public async Task<BaseResponses> UpdateRespuesta([FromBody] UpdateRespuesta request)
        {
            using var connection = _repo.CrearConexion(); //Crear la conexion a la dbb
            var response = await RespuestaHandlers.UpdateRespuesta(connection, request);
            Response.StatusCode = response.code;
            return response;
        }

    }
}
