using MiguelGameDev.DialogueSystem.Commands;
using System.Text;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightGoToCommandFactory : IHighlightGoToCommandFactory
    {
        private readonly StringBuilder _stringBuilder;
        private readonly HighlightStyle _style;

        public HighlightGoToCommandFactory(StringBuilder stringBuilder, HighlightStyle style)
        {
            _stringBuilder = stringBuilder;
            _style = style;
        }

        public IDialogueCommand CreateHighlightCommand(string title, string text)
        {
            return new HighlightGoToCommand(title, _style, _stringBuilder, text);
        }
    }
}
