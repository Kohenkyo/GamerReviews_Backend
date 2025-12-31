using GamerReviewsApi.Repository.Interfaces;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace GamerReviewsApi.EndPoints.JuegoEndP.Handlers
{
    public class postGame
    {
        private readonly IFileStorageService _fileStorage;

        public postGame(IFileStorageService fileStorage)
        {
            _fileStorage = fileStorage;
        }

        public async Task<BaseResponses> PostOneGame(
            IDbConnection connection,
            string nombre,
            string descripcion,
            DateTime fechaCreacion,
            string desarrollador,
            string editor,
            string plataforma,
            IFormFile imagen)
        {
            // ✅ Validar imagen
            var tiposPermitidos = new[] { "image/jpeg", "image/png", "image/webp", "image/jpg" };
            if (imagen == null || !tiposPermitidos.Contains(imagen.ContentType))
            {
                return new BaseResponses(false, (int)HttpStatusCode.BadRequest, "Formato de imagen no permitido.");
            }

            // ✅ Guardar imagen en /uploads
            var nombreImagen = await _fileStorage.SaveImageAsync(imagen, "uploads");

            using (SqlCommand cmd = new SqlCommand("sp_AddNewGame", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // 🔹 Parámetros alineados con el SP corregido
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@descripcion", descripcion);
                cmd.Parameters.AddWithValue("@fechaCreacion", fechaCreacion);
                cmd.Parameters.AddWithValue("@desarrollador", desarrollador);
                cmd.Parameters.AddWithValue("@editor", editor);
                cmd.Parameters.AddWithValue("@plataforma", plataforma);
                cmd.Parameters.AddWithValue("@imagen", nombreImagen);

                if (connection.State != ConnectionState.Open)
                    await ((SqlConnection)connection).OpenAsync();

                // ⚡ Mejor usar ExecuteNonQuery en lugar de ExecuteReader, porque no devuelve filas
                cmd.Parameters.Add("@ReturnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                await cmd.ExecuteNonQueryAsync();

                int resultCode = (int)cmd.Parameters["@ReturnVal"].Value;

                string mensaje = resultCode switch
                {
                    0 => "Juego agregado correctamente",
                    1 => "El juego ya existe",
                    _ => "Error desconocido"
                };

                return new BaseResponses(resultCode == 0,
                                         resultCode == 0 ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest,
                                         mensaje);
            }
        }
    }
}
