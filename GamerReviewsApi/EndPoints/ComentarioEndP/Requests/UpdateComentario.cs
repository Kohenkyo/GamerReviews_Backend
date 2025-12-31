namespace GamerReviewsApi.EndPoints.ComentarioEndP.Requests
{
    public class UpdateComentario
    {
        public int ComentarioId { get; set; }
        public string NuevoTexto { get; set; }
        public byte NuevaPuntuacion { get; set; }
    }
}
