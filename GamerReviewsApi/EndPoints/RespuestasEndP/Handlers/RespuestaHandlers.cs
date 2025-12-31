using GamerReviewsApi.EndPoints.RespuestasEndP.Requests;
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;


namespace GamerReviewsApi.EndPoints.RespuestasEndP.Handlers
{
    public class RespuestaHandlers
    {
        // 🔹 POST: agregar respuesta
        public static async Task<BaseResponses> PostRespuesta(IDbConnection connection,AddRespuesta request)
        {
            if (request == null || string.IsNullOrEmpty(request.Comentario) || request.ComentarioId <= 0 || request.UsuarioId <= 0)
                return new BaseResponses(false, 400, "Datos incompletos");

            
            using (SqlCommand cmd = new SqlCommand("sp_AddRespuesta", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@comentario", request.Comentario);
                cmd.Parameters.AddWithValue("@comentario_id", request.ComentarioId);
                cmd.Parameters.AddWithValue("@usuario_id", request.UsuarioId);

                if (connection.State != ConnectionState.Open) // Abre la conexión si no está ya abierta
                    await ((SqlConnection)connection).OpenAsync();

                var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                await cmd.ExecuteNonQueryAsync();
                int result = (int)returnParameter.Value;

                if (result == 1)
                    return new BaseResponses(false, 400, "El comentario no existe");
                if (result == 2)
                    return new BaseResponses(false, 400, "El usuario no existe");

                return new BaseResponses(true, 200, "Respuesta agregada");
            }
            
        }

        // 🔹 GET: obtener respuestas de un comentario
        public static async Task<DataResponse<IEnumerable<Respuesta>>> GetRespuestasByComentario(IDbConnection connection, int comentarioId, HttpRequest request)
        {
            if (comentarioId <= 0)
                return new DataResponse<IEnumerable<Respuesta>>(false, 400, "ID de comentario inválido");

            
            using (SqlCommand cmd = new SqlCommand("sp_GetRespuestasByComentario", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@comentario_id", comentarioId);

                if (connection.State != ConnectionState.Open) // Abre la conexión si no está ya abierta
                    await ((SqlConnection)connection).OpenAsync();

                var reader = await cmd.ExecuteReaderAsync();

                var lista = new List<Respuesta>();
                while (await reader.ReadAsync())
                {
                    string? fotoArchivo = reader.IsDBNull(4) ? null : reader.GetString(4);

                    lista.Add(new Respuesta
                    {
                        RespuestaId = (int)reader["respuesta_id"],
                        Texto = reader["respuestaTexto"].ToString(),
                        ComentarioId = (int)reader["comentario_id"],
                        UsuarioId = (int)reader["usuario_id"],
                        UsuarioNombre = reader["UsuarioNombre"].ToString(),
                        PerfilUrl = string.IsNullOrEmpty(fotoArchivo)
                        ? null
                        : $"{request.Scheme}://{request.Host}/users/{fotoArchivo}",
                            });
                }

                return new DataResponse<IEnumerable<Respuesta>>(true, 200, "Respuestas obtenidas", lista);
            }
            
        }

        // 🔹 DELETE: baja lógica
        public static async Task<BaseResponses> DeleteRespuesta(IDbConnection connection, int respuestaId)
        {
            if (respuestaId <= 0)
                return new BaseResponses(false, 400, "ID inválido");

            
            using (SqlCommand cmd = new SqlCommand("sp_DeleteRespuesta", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@repuesta_id", respuestaId);

                if (connection.State != ConnectionState.Open) // Abre la conexión si no está ya abierta
                    await ((SqlConnection)connection).OpenAsync();

                var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                await cmd.ExecuteNonQueryAsync();
                int result = (int)returnParameter.Value;

                if (result == 1)
                    return new BaseResponses(false, 404, "Respuesta no encontrada");

                return new BaseResponses(true, 200, "Respuesta eliminada");
            }
            
        }
        // UPDATE: modificar respuesta
        public static async Task<BaseResponses> UpdateRespuesta(IDbConnection connection, UpdateRespuesta request)
        {
            if (request.RespuestaId <= 0 || string.IsNullOrEmpty(request.NuevoTexto))
                return new BaseResponses(false, 400, "Datos incompletos");

            
            using (SqlCommand cmd = new SqlCommand("sp_UpdateRespuesta", (SqlConnection)connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@repuesta_id", request.RespuestaId);
                cmd.Parameters.AddWithValue("@nuevoTexto", request.NuevoTexto);

                if (connection.State != ConnectionState.Open) // Abre la conexión si no está ya abierta
                    await ((SqlConnection)connection).OpenAsync();

                var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                await cmd.ExecuteNonQueryAsync();
                int result = (int)returnParameter.Value;

                if (result == 1)
                    return new BaseResponses(false, 404, "Respuesta no encontrada");

                return new BaseResponses(true, 200, "Respuesta actualizada");
            }
            
        }

    }
}
