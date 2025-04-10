using Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Core.Interfaces.DataAccessors
{
    public interface IUpdatePurchaseDetailItem
    {
        /// <summary>
        /// Updates a PurchaseDetailItem in the database
        /// </summary>
        /// <param name="purchaseDetailItem"></param>
        /// <param name="token"></param>
        /// <returns>Updated model</returns>
        Task<PurchaseDetailItem> UpdatePurchaseDetailItemAsync(PurchaseDetailItem purchaseDetailItem, string userId, CancellationToken token);
    }
}
