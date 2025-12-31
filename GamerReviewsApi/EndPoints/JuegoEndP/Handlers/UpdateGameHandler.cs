using GamerReviewsApi.Repository.Interfaces;
using GamerReviewsApi.Responses;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace GamerReviewsApi.EndPoints.JuegoEndP.Handlers
{
    public class UpdateGameHandler
    {
        private readonly IFileStorageService _fileStorage;

        public UpdateGameHandler(IFileStorageService fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public async Task<BaseResponses> EditGameById(
            IDbConnection connection,
            int juegoId,
            string? nombre,
            string? descripcion,
            DateTime? fechaPublicacion,
            string? desarrollador,
            string? editor,
            string? plataforma,
            IFormFile? imagen,
            string? imagenVieja
            )
        {
            string? nombreImagen = null;

            if (imagen != null)
            {
                var tiposPermitidos = new[] { "image/jpeg", "image/png", "image/webp" };
                if (!tiposPermitidos.Contains(imagen.ContentType))
                {
                    return new BaseResponses(false, (int)HttpStatusCode.BadRequest, "Formato de imagen no permitido.");
                }

                var rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", imagenVieja);
                if (System.IO.File.Exists(rutaImagen))
                {
                    System.IO.File.Delete(rutaImagen);
                }

                nombreImagen = await _fileStorage.SaveImageAsync(imagen, "uploads");
            }

            using (SqlCommand cmd = new SqlCommand("sp_EditeGameById", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@juego_id", juegoId);
                cmd.Parameters.AddWithValue("@nombre", (object?)nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@descripcion", (object?)descripcion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@fechaCreacion", (object?)fechaPublicacion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@desarrollador", (object?)desarrollador ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@editor", (object?)editor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@plataforma", (object?)plataforma ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@imagenURL", (object?)nombreImagen ?? DBNull.Value);

                if (connection.State != ConnectionState.Open)
                    await ((SqlConnection)connection).OpenAsync();

                cmd.Parameters.Add("ReturnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                await cmd.ExecuteNonQueryAsync();

                int resultCode = (int)cmd.Parameters["ReturnVal"].Value;

                string mensaje = resultCode switch
                {
                    0 => "Juego editado correctamente",
                    1 => "Juego no encontrado",
                    _ => "Error desconocido"
                };

                return new BaseResponses(resultCode == 0,
                                         resultCode == 0 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
                                         mensaje);
            }
        }
    }
}
