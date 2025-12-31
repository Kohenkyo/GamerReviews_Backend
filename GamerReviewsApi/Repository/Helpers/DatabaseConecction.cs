using System.Data;
using System.Data.SqlClient;

namespace GamerReviewsApi.Repository.Helpers
{
    public class DatabaseConecction
    {
        private readonly IConfiguration _config; //Inyeccion de dependencia para obtener el string de conexion desde appsettings.json

        public DatabaseConecction(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection CrearConexion()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection")); //Retorna la conexion a la dbb con el string de conexion declarado en appsettings.json
        }
    }
}
