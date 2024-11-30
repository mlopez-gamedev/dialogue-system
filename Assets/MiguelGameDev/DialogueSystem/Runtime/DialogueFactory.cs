using MiguelGameDev.DialogueSystem.Parser;

namespace MiguelGameDev.DialogueSystem
{
    public class DialogueFactory
    {
        private readonly BranchParser _parser;

        public DialogueFactory(BranchParser parser)
        {
            _parser = parser;
        }

        public Dialogue CreateDialogue(string text)
        {
            var mainBranch = _parser.Parse(text);
            var dialogue = new Dialogue(mainBranch);
            dialogue.Setup();
            return dialogue;
        }
    }
}
