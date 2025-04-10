using HandlerClassLibraryProject.Handlers;
using Shouldly;
using System.Diagnostics;

namespace MSTests.UnitTests;

[TestClass]
public class StringIsPalindromeTests
{
    [DataTestMethod]

    //Palindromes
    [DataRow("madam", StringIsPalindrome.Palindrome)]
    [DataRow("Madam", StringIsPalindrome.Palindrome)]
    [DataRow("madam ", StringIsPalindrome.Palindrome)]
    [DataRow("1221", StringIsPalindrome.Palindrome)]
    [DataRow("12321", StringIsPalindrome.Palindrome)]
    [DataRow(" 1221 ", StringIsPalindrome.Palindrome)]
    [DataRow(" 12Ab Ba21 ", StringIsPalindrome.Palindrome)]

    //Not Palindromes
    [DataRow("book", StringIsPalindrome.NotPalindrome)]
    [DataRow("", StringIsPalindrome.NotPalindrome)]
    [DataRow(null, StringIsPalindrome.NotPalindrome)]
    public void HandlerRequest_Success(string str, string expectedResult)
    {
        //arrange
        var handler = new StringIsPalindrome.Handler();

        //act
        var result = handler.Handle(str);

        //assert
        result.ShouldNotBeNullOrWhiteSpace();
        result.ShouldBe(expectedResult);

        //...and because it was requested that I "output" this content, this will output the content to a debugger console.
        Debug.WriteLine(result);
    }
}
