namespace GamerReviewsApi.Repository.Models
{
    public class JuegoPaginado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public string Desarrollador { get; set; }
        public string Editor { get; set; }
        public string Plataforma { get; set; }
        public string ImagenURL { get; set; }

        // Add these for pagination mapping
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}