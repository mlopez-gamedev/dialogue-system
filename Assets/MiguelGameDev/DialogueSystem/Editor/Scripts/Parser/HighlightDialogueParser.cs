using MiguelGameDev.DialogueSystem.Parser;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightDialogueParser
    {
        private HighlightBranchParser _mainParser;

        public HighlightDialogueParser(HighlightBranchParser branchParser)
        {
            _mainParser = branchParser;
        }

        public HighlightDialogue Parse(string text)
        {
            var mainBranch = _mainParser.Parse(text);
            return new HighlightDialogue(mainBranch);
        }
    }

}
