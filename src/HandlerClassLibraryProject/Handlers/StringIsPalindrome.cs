using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandlerClassLibraryProject.Handlers
{
    public class StringIsPalindrome
    {
        // Since these are the expected results, I'm hard-coding them as constants, referencable outside this class, for consistency.
        // Making use of shared resource files are a likely better approach.
        public const string Palindrome = "Palindrome";
        public const string NotPalindrome = "Not Palindrome";

        public class Handler
        {
            public string Handle(string input)
            {
                // Keeping it simple and just returning false if null or empty, rather than throwing an exception.
                if (string.IsNullOrEmpty(input))
                    return NotPalindrome;

                //I am going to make the assumption that we can trim whitespace amd that for text, lower-case is acceptable.
                var cleanString =
                    input
                        .Trim()
                        .ToLowerInvariant();

                // Check if the cleaned input is equal to its reverse
                return cleanString == new string(cleanString.Reverse().ToArray()) ? Palindrome : NotPalindrome;
            }
        }
    }
}
