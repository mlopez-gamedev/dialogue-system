using MiguelGameDev.DialogueSystem.Commands;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightCommand : IDialogueCommand
    {
        protected readonly StringBuilder _stringBuilder;
        private readonly string _highlightedString;
        
        protected IDialogue _dialogue;
        protected IBranch _branch;

        public HighlightCommand(StringBuilder stringBuilder, string highlightedString)
        {
            _stringBuilder = stringBuilder;
            _highlightedString = highlightedString;
        }

        public void CreateBranches(IBranch __)
        {

        }

        public virtual void Setup(IDialogue dialogue, IBranch branch)
        {
            _dialogue = dialogue;
            _branch = branch;
        }

        public virtual void Execute()
        {
            Debug.Log($"{_branch.Index} : {_branch.CurrentCommandIndex} : {_branch.IsMain} => {_highlightedString}");
            if (!_branch.IsMain || _branch.Index != 0 || _branch.CurrentCommandIndex != 0)
            {
                Debug.Log("NeedsAppendLine " + _highlightedString);
                _stringBuilder.AppendLine();
            }
            _stringBuilder.Append(Regex.Unescape(_highlightedString));
        }
    }
}
