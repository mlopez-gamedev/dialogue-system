using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.Parser.Command;
using MiguelGameDev.DialogueSystem.UseCases;
using NSubstitute;
using UnityEngine.Assertions;

namespace MiguelGameDev.DialogueSystem.Tests
{
    public class SingleLineTest
    {
        private readonly string _entry;
        private readonly bool _resultOk;
        private readonly string _message;
        private readonly bool _hasAuthor;
        private readonly string _author;

        public SingleLineTest(string entry, bool resultOk, string message = null, bool hasAuthor = false, string author = null)
        {
            _entry = entry;
            _resultOk = resultOk;
            _message = message;
            _hasAuthor = hasAuthor;
            _author = author;
        }

        public void Test()
        {
            // Arrange
            var showLineUseCase = Substitute.For<IShowLineUseCase>();
            var lineCommandFactory = new LineCommandFactory(showLineUseCase);
            LineCommandParser commandParser = new LineCommandParser(lineCommandFactory);

            //Act
            var command = commandParser.Parse(_entry, new CommandPath(0));

            if (command == null)
            {
                Assert.IsFalse(_resultOk, "Unexpected result: command not created");
                return;
            }

            Assert.IsTrue(_resultOk, "Unexpected result: command shuldn't be created");

            command.Execute();

            //Assert
            showLineUseCase
                .Received(1)
                .ShowLine(Arg.Is<Line>(line => CheckLine(line, _message, _hasAuthor, _author)));
        }

        private bool CheckLine(Line line, string message, bool hasAuthor = false, string author = null)
        {
            var result = line.Text == message && line.HasAuthor == hasAuthor;
            if (result && hasAuthor)
            {
                return line.Author == author;
            }

            return result;
        }
    }
}