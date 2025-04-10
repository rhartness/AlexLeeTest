using Domain.Core.Models.Queries;
using EntityFrameworkProject;
using HandlerClassLibraryProject.DataAccessors;
using HandlerClassLibraryProject.Handlers;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace MSTests.IntegrationTests;

[TestClass]
public class GetPurchaseDetailsTests
{
    AlexLeeDbContext _dbContext;

    public GetPurchaseDetailsTests()
    {
        _dbContext = DbContextFactory.CreateDbContext();
    }

    [TestMethod]
    public void CallHandler_GetAllRecordsEmptyFilter_Success()
    {
        //arrange
        var dataAccessor = new QueryPurchaseDetailItems(_dbContext);

        var queryHandler = new GetPurchaseDetails.Handler(dataAccessor);

        // Empty Query
        var query = new GetPurchaseDetails.Request
        {
            Filter = new PurchaseDetailItemsFilter()
        };

        //act
        var result = queryHandler.Handle(query, new CancellationToken()).Result;

        //assert
        result.Any().ShouldBeTrue();
    }

    [TestMethod]
    public void CallHandler_GetAllRecordsNullFilter_Success()
    {
        //arrange
        var dataAccessor = new QueryPurchaseDetailItems(_dbContext);

        var queryHandler = new GetPurchaseDetails.Handler(dataAccessor);

        // Empty Query
        var query = new GetPurchaseDetails.Request
        {
            Filter = null
        };

        //act
        var result = queryHandler.Handle(query, new CancellationToken()).Result;

        //assert
        result.Any().ShouldBeTrue();
    }

    [TestMethod]
    public void CallHandler_GetFilteredRecords_Success()
    {
        //arrange
        var dataAccessor = new QueryPurchaseDetailItems(_dbContext);

        var queryHandler = new GetPurchaseDetails.Handler(dataAccessor);

        // Empty Query
        var query = new GetPurchaseDetails.Request
        {
            Filter = new PurchaseDetailItemsFilter()
            {
                ItemNumber = 4011
            }
        };

        //act
        var result = queryHandler.Handle(query, new CancellationToken()).Result;

        //assert
        result.Count().ShouldBe(5);
    }

}
