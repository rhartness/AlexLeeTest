using Domain.Core.Interfaces.DataAccessors;
using Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerClassLibraryProject.Handlers
{
    public class UpdatePurchaseDetail
    {
        public class Request
        {
            public PurchaseDetailItem Entity { get; init; }
            public string UserId { get; init; }
        }
        public class Handler
        {
            private readonly IUpdatePurchaseDetailItem _updatePurchaseDetailItem;
            public Handler(IUpdatePurchaseDetailItem updatePurchaseDetailItem)
            {
                _updatePurchaseDetailItem = updatePurchaseDetailItem;
            }
            public async Task<PurchaseDetailItem> Handle(Request request, CancellationToken token)
            {                
                if (request.Entity == null)
                    throw new ArgumentNullException(nameof(request.Entity), "The PurchaseDetailItem entity cannot be null.");
                if (string.IsNullOrWhiteSpace(request.UserId))
                    throw new ArgumentNullException(nameof(request.UserId), "User ID cannot be null or empty.");

                try
                {
                    return await _updatePurchaseDetailItem.UpdatePurchaseDetailItemAsync(request.Entity, request.UserId, token);
                }
                catch
                { 
                    throw;
                }
            }
        }
    }
}
