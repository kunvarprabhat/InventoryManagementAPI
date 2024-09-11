namespace InventoryManagementAPI.Data
{
    public class Inventory
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public DateTime StockInAt { get; set; }
        public int ItemCount { get; set; }
        public string VerifiedBy { get; set; }
        public string VerificationState { get; set; }
    }
}
