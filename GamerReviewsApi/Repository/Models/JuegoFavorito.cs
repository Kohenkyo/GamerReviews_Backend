namespace GamerReviewsApi.Repository.Models
{
    public class JuegoFavorito
    {
        public int JuegoFavoritoId { get; set; } // Id interno del registro (identity)
        public int UsuarioId { get; set; }
        public int JuegoId { get; set; }
        public bool Baja { get; set; }
    }
}
