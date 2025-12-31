namespace GamerReviewsApi.EndPoints.RespuestasEndP.Requests
{
    public class AddRespuesta
    {
        public string Comentario { get; set; }
        public int ComentarioId { get; set; }
        public int UsuarioId { get; set; }
    }
}
