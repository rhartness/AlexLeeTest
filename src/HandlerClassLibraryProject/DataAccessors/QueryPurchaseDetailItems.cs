using Domain.Core.Interfaces.DataAccessors;
using Domain.Core.Models;
using Domain.Core.Models.Queries;
using EntityFrameworkProject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HandlerClassLibraryProject.DataAccessors
{
    public class QueryPurchaseDetailItems : IQueryPurchaseDetailItems
    {
        private AlexLeeDbContext _dbContext;

        public QueryPurchaseDetailItems(AlexLeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PurchaseDetailItem>> GetPurchaseDetailItemsAsync(PurchaseDetailItemsFilter filters, CancellationToken token)
        {
            if (filters == null || filters.IsEmpty())
                return await _dbContext.PurchaseDetailItems
                    .AsNoTracking()
                    .ToListAsync(token);
            else 
                return await _dbContext
                    .PurchaseDetailItems
                    .AsNoTracking()
                    .Where(x => string.IsNullOrWhiteSpace(filters.PurchaseOrderNumber) || x.PurchaseOrderNumber == filters.PurchaseOrderNumber)
                    .Where(x => !filters.ItemNumber.HasValue || x.ItemNumber == filters.ItemNumber)
                    .Where(x => string.IsNullOrWhiteSpace(filters.ItemName) || x.ItemName == filters.ItemName)
                    .Where(x => string.IsNullOrWhiteSpace(filters.ItemDescription) || x.ItemDescription == filters.ItemDescription)
                    .Where(x => !filters.PurchasePrice.HasValue || x.PurchasePrice == filters.PurchasePrice)
                    .Where(x => !filters.PurchaseQuantity.HasValue || x.PurchaseQuantity == filters.PurchaseQuantity)
                    .ToListAsync(token);
        }
    }
}
