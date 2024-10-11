using System;
using System.Text;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Editor
{

    public class HighlightTitleCommand : HighlightCommand
    {
        private readonly string _title;
        private readonly CommandPath _path;
        private readonly string _colorWrong;
        private bool _warning;

        public HighlightTitleCommand(string title, CommandPath path, HighlightStyle style, StringBuilder stringBuilder, string highlightedString) : base(stringBuilder, highlightedString)
        {
            _path = path;
            _title = title;
            _colorWrong = "#" + ColorUtility.ToHtmlStringRGB(style.ErrorColor);
        }

        public override void Setup(IDialogue dialogue, IBranch __)
        {
            try
            {
                dialogue.RegisterTitle(_title, _path);
            }
            catch
            {
                _warning = true;
            }
        }

        public override void Execute()
        {
            base.Execute();
            if (_warning)
            {
                _stringBuilder.Append($" <color={_colorWrong}>(duplicate entry)</color>");
            }
        }
    }
}
