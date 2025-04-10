using Microsoft.AspNetCore.Mvc;
using Domain.Core.Models;
using Domain.Core.Interfaces.DataAccessors;
using Domain.Core.Models.Queries;
using MVCProject.Models.PurchaseDetailItem;
using Microsoft.AspNetCore.Cors;

namespace MVCProject.Controllers
{
    namespace MVCProject.Controllers
    {

        /*
         * TEST NOTES
         * 
         * I probably over engineered this but, even still, this is a less than optimal solution.  
         * You will notice these thin-controllers pass the requests directly into Handler objects.  
         * Preferably, this data would be passed into a messageing system.   The Handler calls 
         * would be located with event/message consumers which can run in another process 
         * (Windows Service or Microservice, or any other form of consumer.)  I've taken a good 
         * bit of time trying to form a "proper" project structure, but this is a portion I'd 
         * like to note.  This is still less than ideal, but it get's the point of the 
         * structure across.
         */


        [ApiController]
        [Route("api/[controller]")]
        [EnableCors("ReactAppPolicy")]
        public class PurchaseDetailItemsController : Controller
        {
            private readonly IQueryPurchaseDetailItems _queryPurchaseDetailItems;
            private readonly IAddPurchaseDetailItem _addPurchaseDetailItem;
            private readonly IUpdatePurchaseDetailItem _updatePurchaseDetailItem;

            public PurchaseDetailItemsController(
                IQueryPurchaseDetailItems queryPurchaseDetailItems, 
                IAddPurchaseDetailItem addPurchaseDetailItem, 
                IUpdatePurchaseDetailItem updatePurchaseDetailItem)
            {
                _queryPurchaseDetailItems = queryPurchaseDetailItems;
                _addPurchaseDetailItem = addPurchaseDetailItem;
                _updatePurchaseDetailItem = updatePurchaseDetailItem;
            }

            [HttpGet]
            [Route("~/PurchaseDetailItems")]
            public async Task<IActionResult> Index(
                string? purchaseOrderNumber = null,
                int? itemNumber = null,
                string? itemName = null,
                string? itemDescription = null,
                decimal? purchasePrice = null,
                int? purchaseQuantity = null)
            {
                var query = new PurchaseDetailItemsFilter()
                {
                    PurchaseOrderNumber = purchaseOrderNumber,
                    ItemNumber = itemNumber,
                    ItemName = itemName,
                    ItemDescription = itemDescription,
                    PurchasePrice = purchasePrice,
                    PurchaseQuantity = purchaseQuantity,
                };

                try
                {
                    var result = await _queryPurchaseDetailItems.GetPurchaseDetailItemsAsync(query, new CancellationToken());
                    return View(result);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"An error occurred while retrieving purchase detail items: {ex.Message}";
                    return View(new List<PurchaseDetailItem>());
                }
            }

            [HttpGet]
            [Route("Search")]
            public async Task<IActionResult> Get(
                string? purchaseOrderNumber = null,
                int? itemNumber = null,
                string? itemName = null,
                string? itemDescription = null,
                decimal? purchasePrice = null,
                int? purchaseQuantity = null)
            {
                var query = new PurchaseDetailItemsFilter()
                {
                    PurchaseOrderNumber = purchaseOrderNumber,
                    ItemNumber = itemNumber,
                    ItemName = itemName,
                    ItemDescription = itemDescription,
                    PurchasePrice = purchasePrice,
                    PurchaseQuantity = purchaseQuantity,
                };

                try
                {
                    var result = await _queryPurchaseDetailItems.GetPurchaseDetailItemsAsync(query, new CancellationToken());
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest($"An error occurred while retrieving purchase detail items: {ex.Message}");
                }
            }

            [HttpPost]
            [Route("Add")]
            public async Task<IActionResult> AddPurchaseDetailItem([FromBody] AddPurchaseDetailItemModel addPurchaseDetailItem)
            {
                if (addPurchaseDetailItem == null)
                {
                    return BadRequest("PurchaseDetailItem cannot be null.");
                }
                var userId = "system";  //Typically, you'd get the request user ID from the authentication context.

                var purchaseDetailItem = new PurchaseDetailItem()
                {
                    PurchaseOrderNumber = addPurchaseDetailItem.PurchaseOrderNumber,
                    ItemNumber = addPurchaseDetailItem.ItemNumber,
                    ItemName = addPurchaseDetailItem.ItemName,
                    ItemDescription = addPurchaseDetailItem.ItemDescription,
                    PurchasePrice = addPurchaseDetailItem.PurchasePrice,
                    PurchaseQuantity = addPurchaseDetailItem.PurchaseQuantity
                };

                try
                {
                    var result = await _addPurchaseDetailItem.AddPurchaseDetailItemAsync(purchaseDetailItem, userId, new CancellationToken());
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest($"An error occurred while adding the purchase detail item: {ex.Message}");
                }
            }

            [HttpPost]
            [Route("Update")]
            public async Task<IActionResult> UpdatePurchaseDetailItem([FromBody] UpdatePurchaseDetailItemModel updatePurchaseDetailItem)
            {
                if (updatePurchaseDetailItem == null)
                {
                    return BadRequest("PurchaseDetailItem cannot be null.");
                }
                var userId = "system";  //Typically, you'd get the request user ID from the authentication context.

                var purchaseDetailItem = new PurchaseDetailItem()
                {
                    PurchaseDetailItemAutoId = updatePurchaseDetailItem.PurchaseDetailItemAutoId,
                    PurchaseOrderNumber = updatePurchaseDetailItem.PurchaseOrderNumber,
                    ItemNumber = updatePurchaseDetailItem.ItemNumber,
                    ItemName = updatePurchaseDetailItem.ItemName,
                    ItemDescription = updatePurchaseDetailItem.ItemDescription,
                    PurchasePrice = updatePurchaseDetailItem.PurchasePrice,
                    PurchaseQuantity = updatePurchaseDetailItem.PurchaseQuantity
                };

                try
                {
                    var result = await _updatePurchaseDetailItem.UpdatePurchaseDetailItemAsync(purchaseDetailItem, userId, new CancellationToken());
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest($"An error occurred while updating the purchase detail item: {ex.Message}");
                }
            }
        }
    }
}
