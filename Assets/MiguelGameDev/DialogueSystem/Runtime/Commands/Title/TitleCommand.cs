using System.Diagnostics;

namespace MiguelGameDev.DialogueSystem.Commands
{
    public class TitleCommand : IDialogueCommand
    {
        private readonly string _title;
        private readonly CommandPath _commandPath;

        public string Title => _title;
        private IDialogue _dialogue;

        public TitleCommand(string title, CommandPath commandPath)
        {
            _title = title;
            _commandPath = commandPath;
        }

        public void CreateBranches(IBranch ___)
        {
            // Nothing to do here
        }

        public void Setup(IDialogue dialogue, IBranch branch)
        {
            _dialogue = dialogue;
            _dialogue.RegisterTitle(_title, _commandPath);
        }

        public void Execute()
        {
            _dialogue.Next();
        }
    }
}
