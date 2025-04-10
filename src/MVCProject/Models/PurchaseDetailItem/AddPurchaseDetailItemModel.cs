namespace MVCProject.Models.PurchaseDetailItem
{
    /// <summary>
    /// This is a specific model for adding new purchase detail items.
    /// </summary>
    public class AddPurchaseDetailItemModel
    {
        public string PurchaseOrderNumber { get; set; } = null!;

        public int ItemNumber { get; set; }

        public string ItemName { get; set; } = null!;

        public string? ItemDescription { get; set; }

        public decimal PurchasePrice { get; set; }

        public int PurchaseQuantity { get; set; }
    }
}
