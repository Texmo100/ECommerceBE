namespace ECommerceBE.Controllers.Utilities
{
    public class PaginationParams
    {
        private int _maxItemsPerPage = 50;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}
