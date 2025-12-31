using GamerReviewsApi.Repository.Models;

public class PaginatedGamesResult
{
    public IEnumerable<JuegoPaginado> Games { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
}