namespace MasterFloorAPP.Models
{
    public class SaleHistoryItem
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateOnly SaleDate { get; set; }
    }
}