using Domain.Core.Models;
using EntityFrameworkProject;
using HandlerClassLibraryProject.DataAccessors;
using HandlerClassLibraryProject.Handlers;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using AutoFixture;

namespace MSTests.IntegrationTests;

[TestClass]
public class AddPurchaseDetailItemTest
{
    AlexLeeDbContext _dbContext;
    Fixture _fixture;

    public AddPurchaseDetailItemTest()
    {
        _dbContext = DbContextFactory.CreateDbContext();
        _fixture = new Fixture();
    }

    [TestMethod]
    public void CallHandler_AddNew_Success()
    {
        //arrange
        var entity = _fixture.Create<PurchaseDetailItem>();
        entity.PurchaseDetailItemAutoId = 0;
        entity.PurchaseOrderNumber = "12345678"; // Fixture is too long.
        var dataAccessor = new AddPurchaseDetailItem(_dbContext);
        var handler = new AddPurchaseDetail.Handler(dataAccessor);

        //act
        var result = handler.Handle(new AddPurchaseDetail.Request
        {
            Entity = entity
        }, 
        "TEST",
        CancellationToken.None).Result;

        //assert
        result.ShouldNotBeNull();
        result.PurchaseDetailItemAutoId.ShouldNotBe(0);
    }

    [TestMethod]
    public void CallHandler_AddNewFromExistingId_Fail()
    {
        //arrange
        var entity = _fixture.Create<PurchaseDetailItem>();
        entity.PurchaseDetailItemAutoId = 1; // This Id exists in the db.
        entity.PurchaseOrderNumber = "12345678"; // Fixture is too long.
        var dataAccessor = new AddPurchaseDetailItem(_dbContext);
        var handler = new AddPurchaseDetail.Handler(dataAccessor);

        //act
        try
        {
            var result = handler.Handle(new AddPurchaseDetail.Request
            {
                Entity = entity
            },
            "TEST", 
            CancellationToken.None).Result;

        }
        catch (Exception e)

        {
            //assert
            e.InnerException.ShouldBeOfType<ArgumentException>();
            e.InnerException.Message.ShouldBe("The PurchaseDetailItem entity is not new and cannot be added. (Parameter 'purchaseDetailItem')");
        }
    }

    [TestMethod]
    public void CallHandler_EmptyRequest_Fail()
    {
        //arrange
        var dataAccessor = new AddPurchaseDetailItem(_dbContext);
        var handler = new AddPurchaseDetail.Handler(dataAccessor);

        //act
        try
        {
            var result = handler.Handle(null, "TEST", CancellationToken.None).Result;

        } catch (Exception e)
        {
            //assert
            e.InnerException.ShouldBeOfType<ArgumentNullException>();
            e.InnerException.Message.ShouldBe("The PurchaseDetailItem entity cannot be null. (Parameter 'Entity')");
        }
    }
}
