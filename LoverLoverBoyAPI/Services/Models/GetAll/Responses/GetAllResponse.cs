using System.ComponentModel.DataAnnotations;

namespace Services.Models.GetAll.Responses;

public class GetAllResponse<T> where T : class
{
    public required IEnumerable<T> Data { get; set; }
    public int Count { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}