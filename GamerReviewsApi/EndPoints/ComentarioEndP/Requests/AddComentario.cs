namespace GamerReviewsApi.EndPoints.ComentarioEndP.Requests
{
    public class AddComentario
    {
        public string Comentario { get; set; }
        public byte Puntuacion { get; set; }
        public int UsuarioId { get; set; }
        public int JuegoId { get; set; }
    }
}
