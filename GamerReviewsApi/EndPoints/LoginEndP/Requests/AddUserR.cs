namespace GamerReviewsApi.EndPoints.LoginEndP.Requests
{
    public class AddUserR
    {
        public string? correo { get; set; }
        public string? contrasena { get; set; }
        public string? nombre { get; set; }
        public DateTime fechaInscripcion { get; set; }

    }
}
