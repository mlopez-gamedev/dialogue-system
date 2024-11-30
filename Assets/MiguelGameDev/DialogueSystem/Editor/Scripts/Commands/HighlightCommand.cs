using MiguelGameDev.DialogueSystem.Commands;
using System.Text;
using System.Text.RegularExpressions;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightCommand : IDialogueCommand
    {
        protected readonly StringBuilder _stringBuilder;
        private readonly string _highlightedString;

        public HighlightCommand(StringBuilder stringBuilder, string highlightedString)
        {
            _stringBuilder = stringBuilder;
            _highlightedString = highlightedString;
        }

        public void CreateBranches(IBranch __)
        {
            // Nothing to do here
        }

        public virtual void Setup(IDialogue __, IBranch ___)
        {
            // Nothing to do here
        }

        public virtual void Execute()
        {
            if (_stringBuilder.Length > 0)
            {
                _stringBuilder.AppendLine();
            }
            _stringBuilder.Append(Regex.Unescape(_highlightedString));
        }
    }
}
