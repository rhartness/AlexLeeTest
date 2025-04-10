using Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Interfaces.DataAccessors
{
    /// <summary>
    /// Interface for adding purchase detail items.
    /// </summary>
    public interface IAddPurchaseDetailItem
    {
        /// <summary>
        /// Inserts a PurchaseDetailItem into the database
        /// </summary>
        /// <param name="purchaseDetailItem"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<PurchaseDetailItem> AddPurchaseDetailItemAsync(PurchaseDetailItem purchaseDetailItem, string userId, CancellationToken token);
    }
}
