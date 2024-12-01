using System.Text;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightRandomBranchCommand : HighlightCommand
    {
        private readonly HighlightSelectBranch[] _branchSelectors;

        public HighlightRandomBranchCommand(StringBuilder stringBuilder, string highlightedString, HighlightSelectBranch[] branchSelectors) : base(stringBuilder, highlightedString)
        {
            _branchSelectors = branchSelectors;
        }

        public override void Setup(IDialogue dialogue, IBranch branch)
        {
            foreach (var selector in _branchSelectors)
            {
                if (selector.Branch != null)
                {
                    selector.Branch.Setup(dialogue, branch);
                }
            }
        }

        public override void Execute()
        {
            base.Execute();
            foreach (var branch in _branchSelectors)
            {
                _stringBuilder.Append(branch.SelectHighlightedText);
                if (branch.Branch != null)
                {
                    branch.Branch.Start();
                }
                
            }
        }
    }
}