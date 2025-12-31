namespace GamerReviewsApi.Repository.Models
{
    public class Respuesta
    {
        public int RespuestaId { get; set; }
        public string Texto { get; set; }
        public int ComentarioId { get; set; }
        public int UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string PerfilUrl { get; set; }
    }
}
