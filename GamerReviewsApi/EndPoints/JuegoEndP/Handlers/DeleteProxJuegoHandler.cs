using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;

namespace GamerReviewsApi.EndPoints.JuegoEndP.Handlers
{
    public class DeleteProxJuegoHandler
    {


        public async Task<BaseResponses> DeleteProxJuego(IDbConnection connection, int id, string imagenVieja)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_Delete_ProxJuego", (SqlConnection)connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    if (connection.State != ConnectionState.Open)
                        await ((SqlConnection)connection).OpenAsync();

                    var rutaImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "carrousel", imagenVieja);
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // El SP devuelve columnas success, error, code, message
                            bool success = Convert.ToInt32(reader["success"]) == 1;
                            int code = Convert.ToInt32(reader["code"]);
                            string message = reader["message"]?.ToString() ?? "Sin mensaje";

                            return new BaseResponses(success, code, message);
                        }
                        else
                        {
                            return new BaseResponses(false, 500, "Respuesta inválida del procedimiento almacenado.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponses(false, 500, $"Error en el servidor: {ex.Message}");
            }
        }
    }
}
