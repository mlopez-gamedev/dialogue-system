﻿using MiguelGameDev.DialogueSystem.Commands;
using System;
using System.Collections.Generic;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightBranch : IBranch
    {
        public IDialogue _dialogue;
        public IBranch _parent;

        private Dictionary<BranchPosition, IBranch> _positionToBranch;
        private IDialogueCommand[] _commandQueue;

        private int _index;
        private BranchPosition[] _path;
        
        private int _currentCommandIndex;

        public IBranch Parent => _parent;
        public bool IsMain => _parent == null;
        public int Index => _index;
        public int CurrentCommandIndex => _currentCommandIndex;
        public BranchPosition[] Path => _path;

        public HighlightBranch(IDialogueCommand[] commands, int index = 0, params BranchPosition[] path)
        {
            _commandQueue = commands;
            _index = index;
            _path = path;
            _positionToBranch = new Dictionary<BranchPosition, IBranch>();
        }

        public void CreateBranches()
        {
            // Nothing to do here
        }


        public void Setup(IDialogue dialogue, IBranch parent = null)
        {
            _dialogue = dialogue;
            _parent = parent;

            foreach (var branch in _positionToBranch.Values)
            {
                branch.Setup(_dialogue, this);
            }

            foreach (var command in _commandQueue)
            {
                command.Setup(_dialogue, this);
            }
        }

        public void Start()
        {
            _currentCommandIndex = 0;
            foreach (var command in _commandQueue)
            {
                command.Execute();
                ++_currentCommandIndex;
            }
        }

        public void Next()
        {
            //
        }

        public bool TrySelectBranch(int branchIndex, out IBranch branch)
        {
            throw new NotImplementedException();
        }

        public void GoTo(int index)
        {
            //
        }

        public IBranch GoToBranchAt(BranchPosition branchPosition)
        {
            throw new NotImplementedException();
        }

        public void RegisterBranch(BranchPosition position, IBranch branch)
        {
            throw new NotImplementedException();
        }
    }
}
