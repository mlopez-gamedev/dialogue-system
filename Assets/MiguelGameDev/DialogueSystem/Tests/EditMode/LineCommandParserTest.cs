using MiguelGameDev.DialogueSystem.Tests;
using NUnit.Framework;

public class EditModeTests
{
    [TestCase("- Miguel: Some Message", true, "Some Message", true, "Miguel")]
    [TestCase("- Miguel\\: el Valiente: Some Message", true, "Some Message", true, "Miguel: el Valiente")]
    [TestCase("- Miguel: Some Message: Another Thing", true, "Some Message: Another Thing", true, "Miguel")]
    [TestCase("- Another Message", true, "Another Message")]
    [TestCase(" - Bad Message", false)]
    [TestCase("Another Bad Message", false)]
    public void TestSingleLine(string entry, bool resultOk, string message = null, bool hasAuthor = false, string author = null)
    {
        var test = new SingleLineTest(entry, resultOk, message, hasAuthor, author);
        test.Test();
    }
}
