using MiguelGameDev.DialogueSystem.Commands;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace MiguelGameDev.DialogueSystem
{
    public class Branch : IBranch
    {
        public IDialogue _dialogue;
        public IBranch _parent;

        private Dictionary<BranchPosition, IBranch> _positionToBranch;
        private IDialogueCommand[] _commandQueue;

        private int _index;
        private BranchPosition[] _path;

        private int _currentCommandIndex;

        public bool IsMain => _parent == null;
        public IBranch Parent => _parent;
        public int Index => _index;
        public int CurrentCommandIndex => _currentCommandIndex;
        public BranchPosition[] Path => _path;

        public Branch(IDialogueCommand[] commands, int index = 0, params BranchPosition[] path)
        {
            _commandQueue = commands;
            _index = index;
            _path = path;
            _positionToBranch = new Dictionary<BranchPosition, IBranch>();
        }

        public void CreateBranches()
        {
            foreach (var command in _commandQueue)
            {
                command.CreateBranches(this);
            }
        }

        public void Setup(IDialogue dialogue, IBranch parent = null)
        {
            Assert.IsNotNull(_commandQueue);
            Assert.IsNotNull(_positionToBranch);

            _dialogue = dialogue;
            _parent = parent;

            foreach (var command in _commandQueue)
            {
                command.Setup(_dialogue, this);
            }

            foreach (var branch in _positionToBranch.Values)
            {
                Assert.IsNotNull(branch);
                branch.Setup(_dialogue, this);
            }
        }

        //private void SetBranchPath()
        //{
        //    var path = new List<int>();

        //    IBranch branch = this;
        //    while (!branch.IsMain)
        //    {
        //        path.Add(_index);
        //        branch = branch.Parent;
        //    }

        //    _path = path.ToArray();
        //}

        public void Start()
        {
            _currentCommandIndex = 0;
            ExecuteCurrentCommand();
        }

        public void Next()
        {
            ++_currentCommandIndex;
            ExecuteCurrentCommand();
        }

        public bool TrySelectBranch(int branchIndex, out IBranch branch)
        {
            var branchPosition = new BranchPosition(_currentCommandIndex, branchIndex);
            if (_positionToBranch.ContainsKey(branchPosition))
            {
                branch = _positionToBranch[branchPosition];
                return true;
            }
            branch = this;
            return false;
        }

        public void GoTo(int index)
        {
            _currentCommandIndex = index;
            ExecuteCurrentCommand();
        }

        public IBranch GoToBranchAt(BranchPosition branchPosition)
        {
            _currentCommandIndex = branchPosition.CommandIndex;
            return _positionToBranch[branchPosition];
        }

        public void RegisterBranch(BranchPosition position, IBranch branch)
        {
            _positionToBranch.Add(position, branch);
        }

        private void ExecuteCurrentCommand()
        {
            Assert.IsNotNull(_commandQueue, "Dialogue has not been setup");

            if (_currentCommandIndex >= _commandQueue.Length)
            {
                if (IsMain)
                {
                    _dialogue.End();
                    return;
                }

                _dialogue.SelectBranch(_parent);
                return;
            }

            _commandQueue[_currentCommandIndex].Execute();
        }
    }
}
