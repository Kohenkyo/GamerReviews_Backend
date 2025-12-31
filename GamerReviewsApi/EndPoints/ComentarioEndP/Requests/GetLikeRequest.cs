namespace GamerReviewsApi.EndPoints.ComentarioEndP.Requests
{

    //esta clase es opcional
    //Si lo querés pasar en query string no hace falta, pero si preferís en body JSON podés usar:
    public class GetLikeRequest
    {
        public int ComentarioId { get; set; }
        public int UsuarioId { get; set; }
    }
}
