namespace GamerReviewsApi.EndPoints.JuegoEndP.Requests
{
    public class AddProxJuegoR
    {
        public string Nombre { get; set; }
        public IFormFile Imagen { get; set; } // porque la subís como archivo
    }
}
