namespace GamerReviewsApi.Repository.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveImageAsync(IFormFile archivo, string carpetaDestino); // Guarda la imagen y devuelve la URL o ruta
    }
}
