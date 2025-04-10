using HandlerClassLibraryProject.Handlers;
using Shouldly;
using System.Diagnostics;

namespace MSTests.UnitTests;

[TestClass]
public class DeepFileStringSearchTests
{
    [DataTestMethod]

    //Palindromes
    [DataRow("Lorem", "Lorem")]
    [DataRow("    ", "SPACES")]
    public void SeachFolder_Success(string term, string suffix)
    {
        //arrange

        //Back up to the project root. Feel free to add files to this dir OR change the path.
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string outputDirectory = Path.Combine("..", "..", "..", "..", "..", "searchResults");
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        string inputDirectory = Path.Combine("..", "..", "..", "..", "..");
        string folderPath = Path.Combine(inputDirectory, "searchMe");
        string outputFile = Path.Combine(outputDirectory, $"search_output_{suffix}.txt");
        string resultSummaryFile = Path.Combine(outputDirectory, $"search_summary_{suffix}.txt");

        // Combine them and resolve to full path
        string absFolderPath= Path.GetFullPath(Path.Combine(baseDirectory, folderPath));
        string absOutputFile = Path.GetFullPath(Path.Combine(baseDirectory, outputFile));


        var request = new DeepFileStringSearch.Request()
        {
            FolderPath = absFolderPath,
            Term = term,
            OutputFile = absOutputFile
        };

        var handler = new DeepFileStringSearch.Handler();

        //act
        var result = handler.Handle(request, CancellationToken.None).Result;

        //assert
        result.ShouldNotBeNull();
        result.OutputFile.ShouldBe(absOutputFile);
        result.FilesProcessed.ShouldBe(5);
        File.Exists(absOutputFile).ShouldBeTrue();

        //output

        string output = $"Files Searched: {result.FilesProcessed}, " +
            $"Output File: {result.OutputFile}, " +
            $"Number of Lines Found: {result.LinesContainingTerm}, " +
            $"Total Occurrences of Term ({term}): {result.TotalOccurencesFound}";
        Debug.WriteLine(output);

        //For review, we'll output a summary file.
        File.WriteAllText(resultSummaryFile, output);
    }
}
