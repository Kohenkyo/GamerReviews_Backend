namespace GamerReviewsApi.Repository.Models
{
    public class UserModel
    {
        public int usuarioId { get; set; }
        public string correo { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
        public int rol { get; set; }
        public string? perfilURL { get; set; }
    }
}
