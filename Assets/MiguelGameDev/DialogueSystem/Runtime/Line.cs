using Unity.VisualScripting.YamlDotNet.Core.Tokens;

namespace MiguelGameDev.DialogueSystem
{

    public readonly struct Line
    {
        public bool HasAuthor { get; }
        public string Author { get; }
        public string Text { get; }

        public Line(string text)
        {
            Author = null;
            HasAuthor = false;
            Text = text;
        }

        public Line(string author, string text)
        {
            Author = author;
            HasAuthor = true;
            Text = text;
        }
    }

}
