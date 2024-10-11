namespace MiguelGameDev.DialogueSystem.Parser
{
    public class DialogueParser
    {
        private BranchParser _branchParser;

        public DialogueParser(BranchParser branchParser)
        {
            _branchParser = branchParser;
        }

        public IDialogue Parse(string text)
        {
            var mainBranch = _branchParser.Parse(text);
            return new Dialogue(mainBranch);
        }
    }
}
