using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HandlerClassLibraryProject.Handlers
{
    public class InterleaveText
    {
        public class Request
        {
            public string String1 { get; init; }
            public string String2 { get; init; }
        }

        public class Handler
        {
            /// <summary>
            /// This method interleaves two strings, taking one character from each string in turn.  
            /// IT ASSUMES that in the interleaving process, if one string is longer than the other, 
            /// it must include the end portion of text for the string that is longer.  That is, if 
            /// the two strings are not the same length, this will not throw an exception.
            /// </summary>
            /// <param name="request"></param>
            /// <returns></returns>
            /// <exception cref="ArgumentNullException"></exception>
            /// <exception cref="ArgumentException"></exception>
            public string Handle(Request request)
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request), "The request cannot be null.");

                if (request.String1 == null || request.String2 == null)
                    throw new ArgumentNullException("Neither String1 nor String2 should be null.");

                StringBuilder result = new StringBuilder();

                //We want to get the max-length of the two strings
                int maxLength = Math.Max(request.String1.Length, request.String2.Length);

                //We need to now loop over the length of the max-length
                for (int i = 0; i < maxLength; i++)
                {
                    // Append char from String1, where relevant
                    if (i < request.String1.Length)
                        result.Append(request.String1[i]);

                    // Append char from String2, where relevant
                    if (i < request.String2.Length)
                        result.Append(request.String2[i]);
                }
                return result.ToString();
            }
        }
    }
}
