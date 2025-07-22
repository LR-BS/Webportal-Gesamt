namespace ISTA.Portal.Application;

public class ResponseWithPagination<T> where T : class
{
    public List<T> Data { get; set; } = new List<T>();
    public int CurrentPage { get; set; }
    public int Filtered { get; set; }
    public int TotalCount { get; set; }
}