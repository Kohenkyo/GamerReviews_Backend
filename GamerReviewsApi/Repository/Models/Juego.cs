namespace GamerReviewsApi.Repository.Models
{
    public class Juego
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public string? Desarrollador { get; set; }
        public string? Editor { get; set; }
        public string? Plataforma { get; set; }
        public string? ImagenURL { get; set; }
        public byte Baja { get; set; } // 👈 NUEVO CAMPO , si se rompre todo, entoces avisaddme a miiii plissss
    }
}
