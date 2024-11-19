namespace KhaKhau.Areas.Admin.Models
    {
    public class StatisticsViewModel
    {
        public int OrdersToday { get; set; }
        public int OrdersLast7Days { get; set; }
        public int OrdersLast10Days { get; set; }

        public List<TopSellingProductModel> TopSellingProducts { get; set; }
        public List<StockSummaryModel> StockSummary { get; set; }
    }

    public class TopSellingProductModel
    {
        public string ProductName { get; set; }
        public int TotalQuantity { get; set; }
    }

    public class StockSummaryModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }


}

