using Domain.Core.Models;
using Domain.Core.Models.Queries;

namespace Domain.Core.Interfaces.DataAccessors
{
    /// <summary>
    /// Interface for querying purchase detail items.
    /// </summary>
    public interface IQueryPurchaseDetailItems
    {
        /// <summary>
        /// Asynchronously retrieves a purchase detail item by its ID.
        /// </summary>
        /// <param name="token">Asynchronous cancellation token</param>
        /// <returns>All db records</returns>
        Task<IEnumerable<PurchaseDetailItem>> GetPurchaseDetailItemsAsync(PurchaseDetailItemsFilter filters, CancellationToken token);
    }
}
