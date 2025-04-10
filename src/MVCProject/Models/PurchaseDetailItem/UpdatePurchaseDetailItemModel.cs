namespace MVCProject.Models.PurchaseDetailItem
{
    public class UpdatePurchaseDetailItemModel
    {
        public long PurchaseDetailItemAutoId { get; set; }
        public string PurchaseOrderNumber { get; set; } = null!;

        public int ItemNumber { get; set; }

        public string ItemName { get; set; } = null!;

        public string? ItemDescription { get; set; }

        public decimal PurchasePrice { get; set; }

        public int PurchaseQuantity { get; set; }
    }
}
