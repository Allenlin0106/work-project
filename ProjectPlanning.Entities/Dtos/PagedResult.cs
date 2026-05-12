using System.Collections.Generic;

namespace ProjectPlanning.Entities.Dtos
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PageCount
        {
            get
            {
                if (PageSize <= 0) return 0;
                return (TotalCount + PageSize - 1) / PageSize;
            }
        }
    }
}
