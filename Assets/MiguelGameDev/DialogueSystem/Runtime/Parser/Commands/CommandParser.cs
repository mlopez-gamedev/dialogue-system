using System;
using System.Text.RegularExpressions;
using MiguelGameDev.DialogueSystem.Commands;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public abstract class CommandParser
    {
        protected const string BranchPrefix = "\t";
        
        public abstract string StartsWith { get; }
        public bool IsEnabled => !string.IsNullOrEmpty(StartsWith);
        private CommandParser _nextParser;
        internal CommandParser NextParser => _nextParser;

        public void SetNextParser(CommandParser nextParser)
        {
            _nextParser = nextParser;
        }

        public IDialogueCommand Parse(string lineCommand, CommandPath commandPath)
        {
            if (TryParse(lineCommand, commandPath, out var command))
            {
                return command;
            }

            return _nextParser?.Parse(lineCommand, commandPath);
        }

        protected abstract bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command);
        
        protected string GetBranchStarts(int level)
        {
            var branchSplitter = "";
            for (int i = 0; i < level; i++)
            {
                branchSplitter += BranchPrefix;
            }
            return branchSplitter;
        }
        
        public string GetBranchSplitter(int level)
        {
            var branchSplitter = "\n";
            for (int i = 0; i < level; i++)
            {
                branchSplitter += BranchPrefix;
            }
            return branchSplitter;
        }
    }
}
