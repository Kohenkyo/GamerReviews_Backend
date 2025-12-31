using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using GamerReviewsApi.EndPoints.ComentarioEndP.Requests;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using static System.Net.Mime.MediaTypeNames;


namespace GamerReviewsApi.EndPoints.ComentarioEndP.Handlers
{
    public class ComentarioHandlers
    {

        // 🔹 POST: agregar comentario
        public static async Task<BaseResponses> PostComentario(IDbConnection connection, AddComentario request)//para el post necesito el AddComentario de la carpeta Requests
        {
            bool check = request != null &&
                         !string.IsNullOrEmpty(request.Comentario) &&
                         request.UsuarioId > 0 &&
                         request.JuegoId > 0;

            if (!check)
                return new BaseResponses(false, (int)HttpStatusCode.BadRequest, "Datos incompletos");

            
            using (SqlCommand cmd = new SqlCommand("sp_AddComentario", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@comentario", request.Comentario);
                cmd.Parameters.AddWithValue("@puntuacion", request.Puntuacion);
                cmd.Parameters.AddWithValue("@usuario_id", request.UsuarioId);
                cmd.Parameters.AddWithValue("@juego_id", request.JuegoId);

                if (connection.State != ConnectionState.Open)   // Abre la conexión si no está ya abierta
                    await ((SqlConnection)connection).OpenAsync();

                var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                await cmd.ExecuteNonQueryAsync();
                int result = (int)returnParameter.Value;

                if (result == 1)
                    return new BaseResponses(false, 400, "Usuario no existe");
                if (result == 2)
                    return new BaseResponses(false, 400, "Juego no existe");

                return new BaseResponses(true, 200, "Comentario agregado");
            }
            
        }

        // 🔹 GET: obtener comentarios por juego
        public static async Task<DataResponse<IEnumerable<Comentario>>> GetComentariosByJuego(IDbConnection connection, int juegoId, HttpRequest request)
        {
            if (juegoId <= 0)
                return new DataResponse<IEnumerable<Comentario>>(false, 400, "ID de juego inválido");

            
            using (SqlCommand cmd = new SqlCommand("sp_GetComentariosByJuego", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@juego_id", juegoId);

                if (connection.State != ConnectionState.Open)   // Abre la conexión si no está ya abierta
                    await ((SqlConnection)connection).OpenAsync();

                var reader = await cmd.ExecuteReaderAsync();

                var lista = new List<Comentario>();

                while (await reader.ReadAsync())
                {
                    var perfilRaw = reader["perfilURL"] as string;

                    lista.Add(new Comentario
                    {
                        ComentarioId = (int)reader["comentario_id"],
                        Texto = reader["comentario"].ToString(),
                        Puntuacion = (byte)reader["puntuacion"],
                        UsuarioId = (int)reader["usuario_id"],
                        JuegoId = (int)reader["juego_id"],
                        UsuarioNombre = reader["UsuarioNombre"].ToString(),
                        PerfilURL = string.IsNullOrEmpty(perfilRaw)
                            ? null
                            : $"{request.Scheme}://{request.Host}/users/{perfilRaw}",
                    });
                }

                return new DataResponse<IEnumerable<Comentario>>(true, 200, "Comentarios obtenidos", lista);
            }
            
        }

        // 🔹 DELETE (baja lógica)
        public static async Task<BaseResponses> DeleteComentario(IDbConnection connection, int comentarioId)
        {
            if (comentarioId <= 0)
                return new BaseResponses(false, 400, "ID inválido");

            
            using (SqlCommand cmd = new SqlCommand("sp_DeleteComentario", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@comentario_id", comentarioId);

                if (connection.State != ConnectionState.Open)   // Abre la conexión si no está ya abierta
                    await ((SqlConnection)connection).OpenAsync();

                var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                await cmd.ExecuteNonQueryAsync();
                int result = (int)returnParameter.Value;

                if (result == 1)
                    return new BaseResponses(false, 404, "Comentario no encontrado");

                return new BaseResponses(true, 200, "Comentario eliminado");
            }
            
        }
        // UPDATE: modificar comentario
        public static async Task<BaseResponses> UpdateComentario(IDbConnection connection, UpdateComentario request)
        {
            if (request.ComentarioId <= 0 || string.IsNullOrEmpty(request.NuevoTexto))
                return new BaseResponses(false, 400, "Datos incompletos");

            
            using (SqlCommand cmd = new SqlCommand("sp_UpdateComentario", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@comentario_id", request.ComentarioId);
                cmd.Parameters.AddWithValue("@nuevoTexto", request.NuevoTexto);
                cmd.Parameters.AddWithValue("@nuevaPuntuacion", request.NuevaPuntuacion);

                if (connection.State != ConnectionState.Open)   // Abre la conexión si no está ya abierta
                    await ((SqlConnection)connection).OpenAsync();

                var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                await cmd.ExecuteNonQueryAsync();
                int result = (int)returnParameter.Value;

                if (result == 1)
                    return new BaseResponses(false, 404, "Comentario no encontrado");

                return new BaseResponses(true, 200, "Comentario actualizado");
            }
            
        }

    }
}
