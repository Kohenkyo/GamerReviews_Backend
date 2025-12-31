using GamerReviewsApi.Repository.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace GamerReviewsApi.Repository.Models
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;

        public FileStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SaveImageAsync(IFormFile archivo, string rutaDestino)
        {
            //Reviso que no este vacio el archivo
            if (archivo == null || archivo.Length == 0)
                throw new ArgumentException("El archivo es nulo o está vacío.", nameof(archivo));

            // Generar nombre único para evitar conflictos
            var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);

            // Crear carpeta si no existe
            var rutaCarpeta = Path.Combine(_env.WebRootPath, rutaDestino); //Le paso como ruta la carpeta wwwroot para poder cargarlo despues en la pag WEB
            if (!Directory.Exists(rutaCarpeta))
                Directory.CreateDirectory(rutaCarpeta);

            // Ruta completa del archivo
            var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

            // Guardar archivo en disco
            using var stream = new FileStream(rutaCompleta, FileMode.Create);
            await archivo.CopyToAsync(stream);

            return nombreArchivo;

        }
    }
}
