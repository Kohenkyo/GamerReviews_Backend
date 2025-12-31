using GamerReviewsApi.Repository.Interfaces;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;

public class postLogin
{
    private readonly IFileStorageService _fileStorage;

    public postLogin(IFileStorageService fileStorage)
    {
        _fileStorage = fileStorage;
    }

    public static async Task<BaseResponses> PostOneLogin(IDbConnection connection, string correo,
                                                  string password, string nombre, DateTime fechaInscr)
    {
        bool CheckLogin = !string.IsNullOrEmpty(correo) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(nombre);

        if (!CheckLogin)
            return new BaseResponses(false, 400, "Los valores no estan completos");

        using var cmd = new SqlCommand("sp_AddNewUserLogin", (SqlConnection)connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@correo", correo);
        cmd.Parameters.AddWithValue("@contrasena", password);
        cmd.Parameters.AddWithValue("@nombre", nombre);
        cmd.Parameters.AddWithValue("@fechaInscripcion", fechaInscr);

        if (connection.State != ConnectionState.Open)
            await ((SqlConnection)connection).OpenAsync();

        cmd.Parameters.Add("@ReturnVal", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
        await cmd.ExecuteNonQueryAsync();
        int resultCode = (int)cmd.Parameters["@ReturnVal"].Value;

        string mensaje = resultCode switch
        {
            0 => "Usuario creado correctamente",
            1 => "Usuario ya existente",
            _ => "Resultado desconocido"
        };

        return new BaseResponses(true, 200, mensaje);
    }
}
