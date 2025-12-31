using Dapper;
using GamerReviewsApi.EndPoints.LoginEndP.Requests;
using GamerReviewsApi.Repository.Models;
using GamerReviewsApi.Responses;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace GamerReviewsApi.EndPoints.LoginEndP.Handlers
{
    public class GetIconUser
    {
        public async static Task<BaseResponses> GetIcon(IDbConnection connection, int usuario_id, HttpRequest request)
        {
            var id = usuario_id;

            var imagen = await connection.QueryFirstOrDefaultAsync<IconUserR>( //Traemos el juego con su id
                "sp_GetIconUSer",
                new { usuario_id = id },
                commandType: CommandType.StoredProcedure
            );

            var data = new IconUserR
            {
                perfilURL = string.IsNullOrEmpty(imagen.perfilURL)
                ? null
                : $"{request.Scheme}://{request.Host}/users/{imagen.perfilURL}",
            };


            return new DataResponse<IconUserR>(true, 200, "Juego encontrado", data);
        }
    }
}
