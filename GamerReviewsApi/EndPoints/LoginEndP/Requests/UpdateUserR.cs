namespace GamerReviewsApi.EndPoints.LoginEndP.Requests
{
    public class UpdateUserR
    {
        public int usuarioId { get; set; }
        public string? correo { get; set; }
        public string? contrasena { get; set; }
        public string? nombre { get; set; }
        public IFormFile? perfilURL { get; set; } // 👈 acá ahora es archivo opcional
        public string? urlVieja { get; set; }
    }
}
