using System.Text;
using MiguelGameDev.DialogueSystem.Commands;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightRandomBranchCommandFactory : IHighlightRandomBranchCommandFactory
    {
        private readonly StringBuilder _stringBuilder;

        public HighlightRandomBranchCommandFactory(StringBuilder stringBuilder)
        {
            _stringBuilder = stringBuilder;
        }

        public IDialogueCommand CreateHighlightCommand(string chances, HighlightSelectBranch[] branchSelectors)
        {
            return new HighlightRandomBranchCommand(_stringBuilder, chances, branchSelectors);
        }
    }
}