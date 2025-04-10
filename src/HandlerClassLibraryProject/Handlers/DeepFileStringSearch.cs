using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HandlerClassLibraryProject.Handlers
{
    public class DeepFileStringSearch
    {
        public class Request
        {
            public string FolderPath { get; init; }
            public string Term { get; init; }
            public string OutputFile { get; init; }
        }

        public class Handler
        {
            public async Task<Result> Handle(Request request, CancellationToken token)
            {
                ValidateRequest(request);

                var fileLocations = new ConcurrentBag<string>(GetFileList(request.FolderPath));
                var foundLines = new ConcurrentDictionary<(string FileName, int LineNumber), ( string Content, int OccurenceCount)> ();

                // NOTE: This can be done better or more thoroughly, especially if you have a high degree of files to process,
                // or if source information is changing, etc.  However, since we need to process all files in the directory and
                // generate summarized information, we are only parallizing the processing of the files, and writing the results
                // to thread safe collections.  There are many ways to do this, but this seems sufficient for the needs.
                Parallel.ForEach(fileLocations, new ParallelOptions 
                { 
                    CancellationToken = token,
                    //MaxDegreeOfParallelism = 1, // FOR TESTING. Comment out for real parallelization
                }, file => SearchFile(file, request.Term, foundLines));

                GenerateOuputFile(request.OutputFile, foundLines);

                return new Result
                {
                    OutputFile = request.OutputFile,
                    FilesProcessed = fileLocations.Count,
                    LinesContainingTerm = foundLines.Count,
                    TotalOccurencesFound = foundLines.Values.Sum(x => x.OccurenceCount)
                };
            }

            private void GenerateOuputFile(string outputFile, ConcurrentDictionary<(string FileName, int LineNumber), (string Content, int OccurenceCount)> foundLines)
            {
               try
                {
                    // Get the directory path from the file path
                    string directoryPath = Path.GetDirectoryName(outputFile);

                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Create a FileStream to write to the file
                    using (FileStream fileStream = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        foreach (var line in foundLines.OrderBy(x => x.Key.FileName).ThenBy(x=>x.Key.LineNumber))
                        {
                            string content = line.Value.Content;
                            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
                            fileStream.Write(contentBytes, 0, contentBytes.Length);
                            fileStream.Flush();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            private void SearchFile(string file, string term, ConcurrentDictionary<(string FileName, int LineNumber), (string Content, int OccurenceCount)> foundLines)
            {
                //Don't search Binary Files!
                if (!IsTextFile(file))
                    return;

                var lines = File.ReadLines(file)
                    .Select((line, index) => new { Line = line, Index = index });

                foreach (var line in lines)
                {
                    int count = Regex.Matches(line.Line, Regex.Escape(term)).Count;
                    if (count > 0)
                    {
                        foundLines.TryAdd((file, line.Index), (line.Line, count));
                    }
                }
            }

            /// <summary>
            /// This is a quick-and-dirty check to see if a file is a text file or not. Binary files, which can contain nulls, can be problematic.
            /// </summary>
            /// <param name="file">Path to file.</param>
            /// <returns>True if expected to be a text file.</returns>
            private bool IsTextFile(string file)
            {
                try
                {
                    using (var stream = File.OpenRead(file))
                    {
                        byte[] buffer = new byte[Math.Min(4096, stream.Length)];
                        stream.Read(buffer, 0, buffer.Length);

                        // Check for null bytes, which rarely appear in text files
                        var result = Array.IndexOf(buffer, (byte)0) == -1;

                        return result;
                    }
                }
                catch
                {
                    return false;
                }
            }

            private List<string> GetFileList(string folderPath)
            {
                return Directory
                    .GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                    .ToList();
            }

            private static void ValidateRequest(Request request)
            {
                if (request == null)
                    throw new ArgumentNullException(nameof(request), "The request cannot be null.");

                //NOTE: We allow for whitespace so that terms like '     ' or '\t' are searchable.
                if (string.IsNullOrWhiteSpace(request.FolderPath))
                    throw new ArgumentException("Folder path cannot be null or empty.", nameof(request.FolderPath));

                if (string.IsNullOrEmpty(request.Term))
                    throw new ArgumentException("Search term cannot be null or empty.", nameof(request.Term));

                if (request.FolderPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                    throw new ArgumentException("Folder path contains invalid characters.", nameof(request.FolderPath));

                string fileName = Path.GetFileName(request.OutputFile);

                var found = request.OutputFile[request.OutputFile.IndexOfAny(Path.GetInvalidFileNameChars())];

                if (string.IsNullOrEmpty(fileName)
                    || fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0
                    || request.OutputFile.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                    throw new ArgumentException("A proper file name must be provided.", nameof(request.FolderPath));
            }
        }

        public class Result
        {
            public string OutputFile { get; init; }
            public int FilesProcessed { get; init; }
            public int LinesContainingTerm { get; init; }   
            public int TotalOccurencesFound { get; init; }
        }
    }
}
