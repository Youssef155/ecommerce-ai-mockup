namespace School.Core.Wrapper
{
    public class PaginatedResult<T>
    {
        public List<T> Data { get; set; }

        public PaginatedResult(List<T> data)
        {
            Data = data;
        }

        public PaginatedResult(bool succeeded, List<T> data = default, List<string> messages = null, int count = 0, int page = 1, int pageSize = 10)
        {
            this.Data = data;
            this.CurrentPage = page;
            this.Succeeded = succeeded;
            this.PageSize = pageSize;
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.TotalCount = count;
        }

        public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize)
        {
            return new(true, data, null, count, page, pageSize);
        }
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public bool HasPreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return CurrentPage < TotalPages;
            }
        }

        public bool Succeeded { get; set; }
    }
}
