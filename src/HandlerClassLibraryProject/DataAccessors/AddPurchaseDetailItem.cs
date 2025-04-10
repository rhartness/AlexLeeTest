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
    public class AddPurchaseDetailItem : IAddPurchaseDetailItem
    {
        private AlexLeeDbContext _dbContext;

        public AddPurchaseDetailItem(AlexLeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PurchaseDetailItem> AddPurchaseDetailItemAsync(PurchaseDetailItem purchaseDetailItem, string userId, CancellationToken token)
        {
            // Data validation works better with a data validator but I'm keeping it simple.
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException("userId", "User ID cannot be null or empty.");

            if (purchaseDetailItem == null)
                throw new ArgumentNullException(nameof(purchaseDetailItem), "PurchaseDetailItem cannot be null.");


            //If this is not a new entity, we throw an exception.
            if (purchaseDetailItem.PurchaseDetailItemAutoId != 0) 
            {
                throw new ArgumentException("The PurchaseDetailItem entity is not new and cannot be added.", nameof(purchaseDetailItem)); 
            }

            //PLEASE NOTE: This is not a preferable way of adding data in a db with EF. But since we have no Db Key, this is our best option.

            var sql = @"
                INSERT INTO dbo.PurchaseDetailItem (
                    PurchaseOrderNumber,  ItemNumber,  ItemName,  ItemDescription,  PurchasePrice,  PurchaseQuantity,  LastModifiedByUser,  LastModifiedDateTime)
                VALUES (
                    @PurchaseOrderNumber, @ItemNumber, @ItemName, @ItemDescription, @PurchasePrice, @PurchaseQuantity, @LastModifiedByUser, GETDATE());";

            var parameters = new[]
            {
                new SqlParameter("@PurchaseOrderNumber", purchaseDetailItem.PurchaseOrderNumber),
                new SqlParameter("@ItemNumber", purchaseDetailItem.ItemNumber),
                new SqlParameter("@ItemName", purchaseDetailItem.ItemName),
                new SqlParameter("@ItemDescription", purchaseDetailItem.ItemDescription),
                new SqlParameter("@PurchasePrice", purchaseDetailItem.PurchasePrice),
                new SqlParameter("@PurchaseQuantity", purchaseDetailItem.PurchaseQuantity),
                new SqlParameter("@LastModifiedByUser", userId),
            };

            // Execute the raw SQL
            await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);


            var newItem = await _dbContext.PurchaseDetailItems
                .Where(x => x.PurchaseOrderNumber == purchaseDetailItem.PurchaseOrderNumber
                         && x.ItemNumber == purchaseDetailItem.ItemNumber
                         && x.PurchasePrice == purchaseDetailItem.PurchasePrice
                         && x.PurchaseQuantity == purchaseDetailItem.PurchaseQuantity)
                .OrderByDescending(x => x.PurchaseDetailItemAutoId)  //Since this is auto-incrmeented, this this close enough for this test.
                .FirstOrDefaultAsync(token);

            return newItem;
        }
    }
}
