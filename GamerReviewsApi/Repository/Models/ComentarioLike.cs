namespace GamerReviewsApi.Repository.Models
{
    public class ComentarioLike
    {
        public int LikeId { get; set; }
        public int ComentarioId { get; set; }
        public int UsuarioId { get; set; }
        public bool Baja { get; set; }
        public DateTime Fecha { get; set; }
    }
   
}
