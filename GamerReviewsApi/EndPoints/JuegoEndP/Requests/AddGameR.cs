namespace GamerReviewsApi.EndPoints.JuegoEndP.Requests
{
    public class AddGameR
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? Desarrollador { get; set; }
        public string? Editor { get; set; }
        public string? Plataforma { get; set; }
        public IFormFile? Imagen { get; set; }
    }
}
