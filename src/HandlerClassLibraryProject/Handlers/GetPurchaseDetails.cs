using Azure.Core;
using Domain.Core.Interfaces.DataAccessors;
using Domain.Core.Models;
using Domain.Core.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerClassLibraryProject.Handlers
{
    public class GetPurchaseDetails
    {
        public class Request
        {
            public PurchaseDetailItemsFilter Filter { get; init; }
        }

        public class Handler
        {
            private readonly IQueryPurchaseDetailItems _queryPurchaseDetailItems;
            public Handler(IQueryPurchaseDetailItems queryPurchaseDetailItems)
            {
                _queryPurchaseDetailItems = queryPurchaseDetailItems;
            }
            public async Task<IEnumerable<PurchaseDetailItem>> Handle(Request request, CancellationToken token)
            {
                try
                {
                    return await _queryPurchaseDetailItems.GetPurchaseDetailItemsAsync(request == null ? null : request.Filter, token);
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
