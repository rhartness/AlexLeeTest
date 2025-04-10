using Domain.Core.Interfaces.DataAccessors;
using Domain.Core.Models;

namespace HandlerClassLibraryProject.Handlers
{
    public class AddPurchaseDetail
    {
        public class Request
        {
            public PurchaseDetailItem Entity { get; init; }
        }

        public class Handler
        {
            private readonly IAddPurchaseDetailItem _addPurchaseDetailItem;
            public Handler(IAddPurchaseDetailItem addPurchaseDetailItem)
            {
                _addPurchaseDetailItem = addPurchaseDetailItem;
            }
            public async Task<PurchaseDetailItem> Handle(Request request, string userId, CancellationToken token)
            {
                if (request == null || request.Entity == null)
                    throw new ArgumentNullException(nameof(request.Entity), "The PurchaseDetailItem entity cannot be null.");

                try
                {
                    return await _addPurchaseDetailItem.AddPurchaseDetailItemAsync(request.Entity, userId, token);
                } 
                catch
                {
                    throw;
                }
            }
        }
    }
}
