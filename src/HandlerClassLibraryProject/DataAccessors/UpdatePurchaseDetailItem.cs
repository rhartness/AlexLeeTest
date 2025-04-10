using Domain.Core.Interfaces.DataAccessors;
using Domain.Core.Models;
using EntityFrameworkProject;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerClassLibraryProject.DataAccessors
{
    public class UpdatePurchaseDetailItem : IUpdatePurchaseDetailItem
    {
        private AlexLeeDbContext _dbContext;
        public UpdatePurchaseDetailItem(AlexLeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PurchaseDetailItem> UpdatePurchaseDetailItemAsync(PurchaseDetailItem purchaseDetailItem, string userId, CancellationToken token)
        {
            //Data validation works better with a data validator but I'm keeping it simple.
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException("userId", "User ID cannot be null or empty.");

            if (!_dbContext.PurchaseDetailItems
                .Any(x => x.PurchaseDetailItemAutoId == purchaseDetailItem.PurchaseDetailItemAutoId))
                throw new KeyNotFoundException($"PurchaseDetailItem with ID {purchaseDetailItem.PurchaseDetailItemAutoId} not found.");

            //PLEASE NOTE: This is not a preferable way of updating data in a db with EF.
            //However, since this data table does not contain a PK, we have to use SQL to make our updates since, theoretically, we might update multiple rows.
            var sql = @"
                UPDATE dbo.PurchaseDetailItem
                SET 
                    PurchaseORderNumber = @PurchaseOrderNumber,
                    ItemNumber = @ItemNumber,
                    ItemName = @ItemName,
                    ItemDescription = @ItemDescription,
                    PurchasePrice = @PurchasePrice,
                    PurchaseQuantity = @PurchaseQuantity,
                    LastModifiedByUser = @LastModifiedByUser,
                    LastModifiedDateTime = GETDATE()
                WHERE 
                    PurchaseDetailItemAutoId = @PurchaseDetailItemAutoId";

            var parameters = new[]
            {
                new SqlParameter("@PurchaseDetailItemAutoId", purchaseDetailItem.PurchaseDetailItemAutoId),
                new SqlParameter("@PurchaseOrderNumber", purchaseDetailItem.PurchaseOrderNumber),
                new SqlParameter("@ItemNumber", purchaseDetailItem.ItemNumber),
                new SqlParameter("@ItemName", purchaseDetailItem.ItemName),
                new SqlParameter("@ItemDescription", purchaseDetailItem.ItemDescription),
                new SqlParameter("@PurchasePrice", purchaseDetailItem.PurchasePrice),
                new SqlParameter("@PurchaseQuantity", purchaseDetailItem.PurchaseQuantity),
                new SqlParameter("@LastModifiedByUser", userId),
            };

            await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);

            return await _dbContext.PurchaseDetailItems
                .FirstOrDefaultAsync(x => x.PurchaseDetailItemAutoId == purchaseDetailItem.PurchaseDetailItemAutoId, token);
        }
    }
}
