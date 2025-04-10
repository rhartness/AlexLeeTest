using Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Models.Queries
{
    public class PurchaseDetailItemsFilter
    {
        public string? PurchaseOrderNumber { get; init; }
        public int? ItemNumber { get; init; }
        public string? ItemName { get; init; }
        public string? ItemDescription { get; init; }
        public decimal? PurchasePrice { get; init; }
        public int? PurchaseQuantity { get; init; }
        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(PurchaseOrderNumber)
                && !ItemNumber.HasValue
                && string.IsNullOrWhiteSpace(ItemName)
                && string.IsNullOrWhiteSpace(ItemDescription)
                && !PurchasePrice.HasValue
                && !PurchaseQuantity.HasValue;
        }
    }
}
