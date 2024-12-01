namespace MiguelGameDev.DialogueSystem
{
    public interface IDialogue
    {
        public event System.Action OnDialogueEnd;
        void Setup();
        void Start();
        void Next();
        void SelectBranch(int branchIndex);
        void SelectBranch(IBranch branch);
        void End();
        void RegisterTitle(string title, CommandPath commandPath);
        CommandPath GetTitlePath(string title);
        void GoTo(CommandPath titlePath);
    }

}
