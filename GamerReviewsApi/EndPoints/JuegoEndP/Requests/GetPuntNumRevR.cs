namespace GamerReviewsApi.EndPoints.JuegoEndP.Requests
{
    public class GetPuntNumRevR
    {
        public int Juego_id { get; set; }
        public double TotalPuntos { get; set; }
        public double CantidadReviews { get; set; }
        public double Calificacion { get; set; }
    }
}
