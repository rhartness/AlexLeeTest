using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Core.Models;

public partial class PurchaseDetailItem
{
    [Key]
    public long PurchaseDetailItemAutoId { get; set; }

    public string PurchaseOrderNumber { get; set; } = null!;

    public int ItemNumber { get; set; }

    public string ItemName { get; set; } = null!;

    public string? ItemDescription { get; set; }

    public decimal PurchasePrice { get; set; }

    public int PurchaseQuantity { get; set; }

    public string LastModifiedByUser { get; set; } = null!;

    public DateTime LastModifiedDateTime { get; set; }
}
