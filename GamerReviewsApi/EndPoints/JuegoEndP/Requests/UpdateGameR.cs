namespace GamerReviewsApi.EndPoints.JuegoEndP.Requests
{
    public class UpdateGameR
    {
        public int JuegoId { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public string? Desarrollador { get; set; }
        public string? Editor { get; set; }
        public string? Plataforma { get; set; }
        public IFormFile? Imagen { get; set; } // 👈 para subir archivo
        public string? ImagenVieja { get; set; }
    }
}
