using GamerReviewsApi.EndPoints.LoginEndP.Handlers;
using GamerReviewsApi.EndPoints.LoginEndP.Requests;
using GamerReviewsApi.Repository.Helpers;
using GamerReviewsApi.Repository.Interfaces;
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace GamerReviewsApi.EndPoints.LoginEndP
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        protected readonly IFileStorageService _fileStorage; //intyecta


        private readonly DatabaseConecction _repo; //Inyeccion de dependencia para obtener la conexion a la dbb


        public LoginController(DatabaseConecction repo, IFileStorageService fileStorage)
        {
            _repo = repo;
            _fileStorage = fileStorage;
        }

        [HttpPost]
        [Route("create-user")]
        public async Task<BaseResponses> CreateUser([FromBody] AddUserR request)
        {
            using var connection = _repo.CrearConexion(); //Crear la conexion a la dbb
            BaseResponses responses = await postLogin.PostOneLogin(connection, request.correo, request.contrasena, request.nombre, 
                request.fechaInscripcion); //Llamar al handler para crear un nuevo usuario
            Response.StatusCode = responses.code; //Asignar el codigo de respuesta HTTP
            return responses;
        }

        [HttpPost]
        [Route("login")]
        public async Task<BaseResponses> Login([FromBody] LoginIDRequest request, [FromServices] JWTGen jwtGen)
        {
            using var connection = _repo.CrearConexion();
            BaseResponses responses = await LoginHandler.DoLogin(connection, request.Correo, request.Contrasena, jwtGen);
            Response.StatusCode = responses.code;
            return responses;
        }

        [HttpGet]
        [Route("get-user")]
        [Authorize]
        public async Task<BaseResponses> GetUser([FromQuery] int usuario_id)
        {
            using var connection = _repo.CrearConexion();
            var response = await GetUserHandler.GetOneUser(connection, usuario_id, Request);
            Response.StatusCode = response.code;
            return response;
        }

        [HttpPatch]
        [Route("update-user")]
        [Authorize]
        public async Task<BaseResponses> UpdateUser([FromForm] UpdateUserR request, [FromServices] UpdateUserHandler handler)
        {
            using var connection = _repo.CrearConexion();
            var response = await handler.UpdateUser(
                connection,
                request.usuarioId,
                request.correo,
                request.contrasena,
                request.nombre,
                request.perfilURL,
                request.urlVieja
            );
            Response.StatusCode = response.code;
            return response;
        }

        [HttpGet]
        [Route("get-all-users")]
        [Authorize]
        public async Task<BaseResponses> GetAllUsers()
        {
            using var connection = _repo.CrearConexion();
            var responses = await GetAllUsersHandler.GetAllUsers(connection, Request);
            Response.StatusCode = responses.code;
            return responses;
        }

        [HttpPut("change-user-state")]
        [Authorize]
        public async Task<BaseResponses> ChangeUserState([FromQuery] int id, [FromQuery] int baja)
        {
            var connection = _repo.CrearConexion();
            var result = await ChangeUserStateHandler.ChangeUserState(connection, id, baja);
            Response.StatusCode = result.code;
            return result;
        }

        [HttpGet]
        [Route("getIconXUser")]
        [Authorize]
        public async Task<BaseResponses> GetIconXUser([FromQuery]int usuario_id)
        {
            using var connection = _repo.CrearConexion();
            var responses = await GetIconUser.GetIcon(connection, usuario_id, Request);
            Response.StatusCode = responses.code;
            return responses;
        }

        [HttpGet]
        [Route("getMisReviews")]
        //[Authorize]
        public async Task<BaseResponses> getMisReviews([FromQuery] int usuario_id)
        {
            using var connection = _repo.CrearConexion();
            var responses = await GetMisReviewsH.GetReviews(connection, usuario_id);
            Response.StatusCode = responses.code;
            return responses;
        }
    }
}
