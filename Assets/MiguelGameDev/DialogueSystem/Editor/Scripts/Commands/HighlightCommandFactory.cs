using MiguelGameDev.DialogueSystem.Commands;
using System.Text;

namespace MiguelGameDev.DialogueSystem.Editor
{

    public class HighlightCommandFactory : IHighlightCommandFactory
    {
        private readonly StringBuilder _stringBuilder;

        public HighlightCommandFactory(StringBuilder stringBuilder)
        {
            _stringBuilder = stringBuilder;
        }

        public IDialogueCommand CreateHighlightCommand(string text)
        {
            return new HighlightCommand(_stringBuilder, text);
        }
    }
}
