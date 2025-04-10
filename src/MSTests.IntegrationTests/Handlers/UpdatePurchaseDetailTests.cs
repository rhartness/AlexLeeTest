using AutoFixture;
using EntityFrameworkProject;
using HandlerClassLibraryProject.DataAccessors;
using HandlerClassLibraryProject.Handlers;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.ComponentModel.DataAnnotations;

namespace MSTests.IntegrationTests;

[TestClass]
public class UpdatePurchaseDetailTests
{
    AlexLeeDbContext _dbContext;

    public UpdatePurchaseDetailTests()
    {
        _dbContext = DbContextFactory.CreateDbContext();
    }

    [TestMethod]
    public void Call_Handler_UpdateExisting_Success()
    {
        //arrange
        var entity = _dbContext.PurchaseDetailItems.First();
        var originalModifiedDate = entity.LastModifiedDateTime;
        var dataAccessor = new UpdatePurchaseDetailItem(_dbContext);
        var handler = new UpdatePurchaseDetail.Handler(dataAccessor);

        //act
        var result = handler.Handle(new UpdatePurchaseDetail.Request
        {
            Entity = entity,
            UserId = "TEST"
        }, new CancellationToken()).Result;

        //assert
        result.ShouldNotBeNull();
        result.PurchaseDetailItemAutoId.ShouldBe(entity.PurchaseDetailItemAutoId);
        result.LastModifiedDateTime.ShouldBeGreaterThan(originalModifiedDate);
    }
}
