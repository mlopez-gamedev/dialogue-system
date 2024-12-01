using System.Collections.Generic;
using UnityEngine.Assertions;

namespace MiguelGameDev.DialogueSystem
{
    public class Dialogue : IDialogue
    {
        private IBranch _mainBranch;
        private Dictionary<string, CommandPath> _titles;

        private IBranch _currentBranch;
        public event System.Action OnDialogueEnd;

        public Dialogue(IBranch mainBranch)
        {
            _mainBranch = mainBranch;
            _titles = new Dictionary<string, CommandPath>();
        }

        public void Setup()
        {
            _mainBranch.CreateBranches();
            _mainBranch.Setup(this);
        }

        public void Start()
        {
            _currentBranch = _mainBranch;
            _currentBranch.Start();
        }

        public void Next()
        {
            _currentBranch.Next();
        }

        public void SelectBranch(int branchIndex)
        {
            if (_currentBranch.TrySelectBranch(branchIndex, out _currentBranch))
            {
                _currentBranch.Start();
                return;
            }
            _currentBranch.Next();
        }

        public void SelectBranch(IBranch branch)
        {
            _currentBranch = branch;
            _currentBranch.Next();
        }

        public void End()
        {
            OnDialogueEnd?.Invoke();
        }

        public void RegisterTitle(string title, CommandPath titlePath)
        {
            Assert.IsFalse(_titles.ContainsKey(title));

            _titles.Add(title, titlePath);
        }

        public CommandPath GetTitlePath(string title)
        {
            Assert.IsTrue(_titles.ContainsKey(title));
            return _titles[title];
        }

        public void GoTo(CommandPath commandPath)
        {
            var branch = _mainBranch;
            foreach (BranchPosition branchPosition in commandPath.BranchPositions)
            {
                branch = branch.GoToBranchAt(branchPosition);
            }
            _currentBranch = branch;
            _currentBranch.GoTo(commandPath.CommandIndex);
        }
    }

}
