using HandlerClassLibraryProject.Handlers;
using Shouldly;

namespace MSTests.UnitTests;

[TestClass]
public class InterleaveTextTests
{
    [TestMethod]
    public void InterleaveText_SameLength_Good()
    {
        //arrange
        var request = new InterleaveText.Request()
        {
            String1 = "abc",
            String2 = "123"
        };

        var handler = new InterleaveText.Handler();

        //act
        var result = handler.Handle(request);

        //assert
        result.ShouldBe("a1b2c3");
    }

    [TestMethod]
    public void InterleaveText_String1Longer_Good()
    {
        //arrange
        var request = new InterleaveText.Request()
        {
            String1 = "abcde",
            String2 = "123"
        };

        var handler = new InterleaveText.Handler();

        //act
        var result = handler.Handle(request);

        //assert
        result.ShouldBe("a1b2c3de");
    }

    [TestMethod]
    public void InterleaveText_String2Longer_Good()
    {
        //arrange
        var request = new InterleaveText.Request()
        {
            String1 = "abc",
            String2 = "12345"
        };

        var handler = new InterleaveText.Handler();

        //act
        var result = handler.Handle(request);

        //assert
        result.ShouldBe("a1b2c345");
    }

    [TestMethod]
    public void InterleaveText_String1Empty_Good()
    {
        //arrange
        var request = new InterleaveText.Request()
        {
            String1 = "",
            String2 = "123"
        };

        var handler = new InterleaveText.Handler();

        //act
        var result = handler.Handle(request);

        //assert
        result.ShouldBe("123");
    }

    [TestMethod]
    public void InterleaveText_String2Empty_Good()
    {
        //arrange
        var request = new InterleaveText.Request()
        {
            String1 = "abc",
            String2 = ""
        };

        var handler = new InterleaveText.Handler();

        //act
        var result = handler.Handle(request);

        //assert
        result.ShouldBe("abc");
    }

    [TestMethod]
    public void InterleaveText_NullRequest_Fail()
    {
        //arrange
        var handler = new InterleaveText.Handler();

        try
        {
            //act
            var result = handler.Handle(null);
        }
        catch (ArgumentNullException ex)
        {
            //assert
            ex.ShouldNotBeNull();
            ex.Message.ShouldBe("The request cannot be null. (Parameter 'request')");
        }
    }

    [TestMethod]
    public void InterleaveText_NullString_Fail()
    {
        //arrange
        var request = new InterleaveText.Request()
        {
            String1 = null,
            String2 = "123"
        };
        var handler = new InterleaveText.Handler();
        //act
        try
        {
            var result = handler.Handle(request);
        }
        catch (ArgumentException ex)
        {
            //assert
            ex.ShouldNotBeNull();
            ex.Message.ShouldBe("Value cannot be null. (Parameter 'Neither String1 nor String2 should be null.')");
        }
    }
}
