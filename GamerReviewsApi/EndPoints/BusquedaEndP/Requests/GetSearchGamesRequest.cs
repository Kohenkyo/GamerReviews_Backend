using GamerReviewsApi.Repository.Models;

namespace GamerReviewsApi.EndPoints.BusquedaEndP.Requests
{
    public class GetSearchGamesRequest
    {
        public class GetGameR
        {
            public int Id { get; set; }
            public string? Nombre { get; set; }
            public string? Descripcion { get; set; }
            public string? FechaPublicacion { get; set; }
            public string? Desarrollador { get; set; }
            public string? Editor { get; set; }
            public string? Plataforma { get; set; }
            public string? ImagenURL { get; set; }

            public List<Tag>? Tags { get; set; } //Creamos una lista de tags(categorias  de juego)
        }
    }
}
