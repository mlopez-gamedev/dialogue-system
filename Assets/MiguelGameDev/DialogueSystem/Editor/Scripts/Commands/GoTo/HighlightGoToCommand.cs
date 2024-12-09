using Codice.Client.BaseCommands.Merge.Xml;
using System.Text;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightGoToCommand : HighlightCommand
    {
        private readonly string _title;
        private readonly string _colorWrong;

        public HighlightGoToCommand(string title, HighlightStyle style, StringBuilder stringBuilder, string highlightedString) : base(stringBuilder, highlightedString)
        {
            _title = title;
            _colorWrong = "#" + ColorUtility.ToHtmlStringRGB(style.ErrorColor);
        }

        public override void Execute()
        {
            base.Execute();
            try
            {
                _dialogue.GetTitlePath(_title);
            }
            catch
            {
                _stringBuilder.Append($" <color={_colorWrong}>(unregistered title)</color>");
            }
        }
    }
}
