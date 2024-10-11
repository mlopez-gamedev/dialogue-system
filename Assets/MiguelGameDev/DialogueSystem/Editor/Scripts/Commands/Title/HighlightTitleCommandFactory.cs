using MiguelGameDev.DialogueSystem.Commands;
using System.Text;

namespace MiguelGameDev.DialogueSystem.Editor
{


    public class HighlightTitleCommandFactory : IHighlightTitleCommandFactory
    {
        private readonly StringBuilder _stringBuilder;
        private readonly HighlightStyle _style;

        public HighlightTitleCommandFactory(StringBuilder stringBuilder, HighlightStyle style)
        {
            _stringBuilder = stringBuilder;
            _style = style;
        }

        public IDialogueCommand CreateHighlightCommand(string title, CommandPath commandPath, string text)
        {
            return new HighlightTitleCommand(title, commandPath, _style, _stringBuilder, text);
        }
    }
}
