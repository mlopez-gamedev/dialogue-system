namespace MiguelGameDev.DialogueSystem.Commands
{
    public class GoToCommand : IDialogueCommand
    {
        private readonly string _goToTitle;

        private IDialogue _dialogue;
        private CommandPath _goToPath;

        public GoToCommand(string goToTitle)
        {
            _goToTitle = goToTitle;
        }

        public void CreateBranches(IBranch _)
        {
            // Nothing to do here
        }

        public void Setup(IDialogue dialogue, IBranch branch)
        {
            _dialogue = dialogue;
            _goToPath = _dialogue.GetTitlePath(_goToTitle);
        }

        public void Execute()
        {
            _dialogue.GoTo(_goToPath);
        }
    }
}
