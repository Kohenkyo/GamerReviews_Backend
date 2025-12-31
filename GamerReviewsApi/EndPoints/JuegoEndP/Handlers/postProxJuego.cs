using GamerReviewsApi.Repository.Interfaces;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace GamerReviewsApi.EndPoints.JuegoEndP.Handlers
{
    public class postProxJuego
    {
        protected readonly IFileStorageService _fileStorage;

        public postProxJuego(IFileStorageService fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public async Task<BaseResponses> PostOneProxJuego(IDbConnection connection, string nombre, IFormFile imagen)
        {
            var tiposPermitidos = new[] { "image/jpeg", "image/png", "image/webp" };

            if (!tiposPermitidos.Contains(imagen.ContentType))
            {
                return new BaseResponses(false, (int)HttpStatusCode.BadRequest, "Formato de imagen no permitido.");
            }
            else
            {
                var nombreImagen = await _fileStorage.SaveImageAsync(imagen, "carrousel");

                using (SqlCommand cmd = new SqlCommand("sp_AddNewProxJuego", (SqlConnection)connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@imagen", nombreImagen);

                    if (connection.State != ConnectionState.Open)
                        await ((SqlConnection)connection).OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        return new BaseResponses(true, (int)HttpStatusCode.OK, "Próximo juego agregado");
                    }
                }
            }
        }
    }
}
