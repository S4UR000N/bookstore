namespace Associated.Application.Common.Model.Response
{
    public class PaginatedResponseModel<T> : ResponseModel<T>
    {
        public long Page { get; set; } = 0;
        public long TotalPages { get; set; } = 0;
        public long TotalResults { get; set; } = 0;
    }
}
