using System;
namespace FinanceApp.Model
{
    public class PaginationModel
    {
        const int maxPageSize = 100;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        public string QuerySearch { get; set; }
    }
}
