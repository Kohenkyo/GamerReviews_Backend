namespace GamerReviewsApi.EndPoints.JuegoEndP.Requests
{
    public class FavoriteGame
    {
        public int UsuarioId { get; set; }
        public int JuegoId { get; set; }
        public bool BotonCheck { get; set; } // true = agregar/reactivar, false = quitar (baja = 1)
    }
}
