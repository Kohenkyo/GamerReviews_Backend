using System.Runtime.CompilerServices;

namespace GamerReviewsApi.Repository.Models
{
    public class Comentario
    {
        public int ComentarioId { get; set; }
        public string Texto { get; set; }
        public int Puntuacion { get; set; }
        public int UsuarioId { get; set; }
        public int JuegoId { get; set; }
        public DateTime Fecha { get; set; }
        public string UsuarioNombre { get; set; } // JOIN con tabla usuarios
        public string PerfilURL { get; set; }
    }



}