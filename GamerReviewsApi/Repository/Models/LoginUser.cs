namespace GamerReviewsApi.Repository.Models
{
    public class LoginUser
    {
        public int usuarioId { get; set; }
        public string usuario { get; set; } = string.Empty; // nombre
        public string correo { get; set; } = string.Empty;
        //  public IFormFile perfilURL { get; set; }
        public string perfilURL { get; set; }

        public string foto { get; set; }
        public byte rol { get; set; }
        public byte baja { get; set; }
    }
}
