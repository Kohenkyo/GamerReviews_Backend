using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;

namespace GamerReviewsApi.EndPoints.JuegoEndP.Handlers
{
    public class getProxJuegos
    {
        public async Task<BaseResponses> AllProxJuegos(IDbConnection connection, HttpRequest request)
        {
            var lista = new List<ProxJuego>();

            using (SqlCommand cmd = new SqlCommand("SELECT Id, Nombre, FotoUrl FROM proxJuegos", (SqlConnection)connection))
            {
                if (connection.State != ConnectionState.Open)
                    await ((SqlConnection)connection).OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        // Leemos el nombre de archivo directamente de la DB
                        var fotoArchivo = reader.GetString(2);

                        lista.Add(new ProxJuego
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            // Construimos la URL completa igual que en getGames
                            FotoUrl = $"{request.Scheme}://{request.Host}/carrousel/{fotoArchivo}",
                            FotoVieja = fotoArchivo.ToString()
                        });
                    }
                }
            }

            return new DataResponse<List<ProxJuego>>(true, 200, "Lista obtenida con éxito", lista);
        }
    }
}
