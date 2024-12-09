using MiguelGameDev.DialogueSystem;
using System.Collections.Generic;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightDialogue : IDialogue
    {
        public event System.Action OnDialogueEnd;

        private readonly IBranch _mainBranch;
        private readonly Dictionary<string, CommandPath> _titles;

        public HighlightDialogue(IBranch mainBranch)
        {
            _mainBranch = mainBranch;
            _titles = new Dictionary<string, CommandPath>();
        }

        public void Setup()
        {
            _mainBranch.Setup(this);
        }

        public void Start()
        {
            _mainBranch.Start();
        }

        public void Next()
        {
            //
        }

        public void End()
        {
            //
        }

        public void GoTo(CommandPath titlePath)
        {
            //
        }

        public void RegisterTitle(string title, CommandPath commandPath)
        {
            _titles.Add(title, commandPath);
        }

        public CommandPath GetTitlePath(string title)
        {
            return _titles[title];
        }

        public void SelectBranch(int branchIndex)
        {
            //
        }

        public void SelectBranch(IBranch branch)
        {
            //
        }
    }
}
