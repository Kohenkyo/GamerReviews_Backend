using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;

namespace GamerReviewsApi.EndPoints.TagsEndP.Handlers
{
    public class AddTag
    {
        public async static Task<BaseResponses> AddOneTag(IDbConnection connection ,string nombre)
        {
            bool CheckLogin = !string.IsNullOrEmpty(nombre);

            if (!CheckLogin)
                return new BaseResponses(false, 400, "Los valores no estan completos");

            using var cmd = new SqlCommand("sp_AddNewTags", (SqlConnection)connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@nombre", nombre);

            if (connection.State != ConnectionState.Open)
                await ((SqlConnection)connection).OpenAsync();

            cmd.Parameters.Add("@ReturnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            await cmd.ExecuteNonQueryAsync();
            int resultCode = (int)cmd.Parameters["@ReturnVal"].Value;

            string mensaje = resultCode switch
            {
                0 => "Tag creado correctamente",
                1 => "Tag ya existente",
                _ => "Resultado desconocido"
            };

            return new BaseResponses(true, 200, mensaje);
        }

        public async static Task<BaseResponses> TagXgame(IDbConnection connection, int? juego_id, int? tag_id)
        {
            using var cmd = new SqlCommand("sp_InsertTagXGame", (SqlConnection)connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@juego_id", juego_id);
            cmd.Parameters.AddWithValue("@tag_id", tag_id);

            if (connection.State != ConnectionState.Open)
                await ((SqlConnection)connection).OpenAsync();

            cmd.Parameters.Add("@ReturnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
            await cmd.ExecuteNonQueryAsync();
            int resultCode = (int)cmd.Parameters["@ReturnVal"].Value;

            string mensaje = resultCode switch
            {
                0 => "Tag insertado correctamente",
                1 => "Tag ya existente",
                _ => "Resultado desconocido"
            };

            return new BaseResponses(true, 200, mensaje);
        }
    }
}
