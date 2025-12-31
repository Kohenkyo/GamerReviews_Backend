using GamerReviewsApi.Repository.Interfaces;
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace GamerReviewsApi.EndPoints.LoginEndP.Handlers
{
    public class UpdateUserHandler
    {
        private readonly IFileStorageService _fileStorage;

        public UpdateUserHandler(IFileStorageService fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public async Task<BaseResponses> UpdateUser(
            IDbConnection connection,
            int usuarioId,
            string? correo,
            string? contrasena,
            string? nombre,
            IFormFile? imagen,
            string? urlvieja) // 👈 recibe archivo
        {
            string? nombreImagen = null;

            if (imagen != null && imagen.Length > 0)
            {
                var tiposPermitidos = new[] { "image/jpeg", "image/png", "image/webp" };
                if (!tiposPermitidos.Contains(imagen.ContentType))
                {
                    return new BaseResponses(false, (int)HttpStatusCode.BadRequest, "Formato de imagen no permitido.");
                }

                var rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "users", urlvieja);
                if (System.IO.File.Exists(rutaImagen))
                {
                    System.IO.File.Delete(rutaImagen);
                }

                // Guardamos nueva imagen
                nombreImagen = await _fileStorage.SaveImageAsync(imagen, "users");
            }

            using (SqlCommand cmd = new SqlCommand("sp_UpdateUser", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@usuario_id", usuarioId);
                cmd.Parameters.AddWithValue("@correo", (object?)correo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@contrasena", (object?)contrasena ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nombre", (object?)nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@perfilURL", (object?)nombreImagen ?? DBNull.Value);

                if (connection.State != ConnectionState.Open)
                    await ((SqlConnection)connection).OpenAsync();

                cmd.Parameters.Add("ReturnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                await cmd.ExecuteNonQueryAsync();

                int resultCode = (int)cmd.Parameters["ReturnVal"].Value;

                string mensaje = resultCode switch
                {
                    0 => "Usuario actualizado correctamente",
                    1 => "Usuario no encontrado",
                    _ => "Error desconocido"
                };

                return new BaseResponses(resultCode == 0,
                                         resultCode == 0 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotFound,
                                         mensaje);
            }
        }
    }
}
