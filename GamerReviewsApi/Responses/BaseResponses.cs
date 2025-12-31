using GamerReviewsApi.Repository.Models;

namespace GamerReviewsApi.Responses
{
    public class BaseResponses
    {
        public bool success { get; set; } //Indica si la operación fue exitosa (valor booleano).
        public bool error { get; set; } //Indica si hubo un error (derivado de success ).
        public int code { get; set; } //Un código numérico que puede representar el estado de la respuesta (por ejemplo, 200 para éxito, 400 para error, etc.).
        public string message { get; set; } //Mensaje adicional que describe la respuesta (por ejemplo, "Operación exitosa" o "Error: datos no encontrados").

        public BaseResponses(bool success, int code, string message) //Toma tres parámetros para inicializar las propiedades.
        {
            this.success = success; // Cuando success es true , error será automáticamente false, y viceversa.
            this.error = !success;
            this.code = code;
            this.message = message;
        }
    }

    public class DataResponse<T> : BaseResponses //Es una clase genérica que hereda de BaseResponses
    {
        public new T data { get; set; } = default; //está diseñada para almacenar información específica que
                                                   //quieres incluir en una respuesta. Como la clase es genérica, el
                                                   //tipo de datos que puede contener se define al momento de instanciarla,
                                                   //permitiendo que  sea cualquier tipo de objeto, lista, o incluso tipos simples
                                                   //como números o cadenas.

        public DataResponse(bool success, int code, string message, T data = default) : base(success, code, message)
        {
            this.data = data;
        }
    }
}
