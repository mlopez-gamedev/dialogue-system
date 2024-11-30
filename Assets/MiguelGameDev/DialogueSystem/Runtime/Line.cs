using System;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

namespace MiguelGameDev.DialogueSystem
{

    public readonly struct Line
    {
        public bool HasAuthor { get; }
        public string Author { get; }
        public string Message { get; }
        public string Metadata { get; }
        public bool HasMetadata => !string.IsNullOrEmpty(Metadata);

        public Line(string message, string metadata)
        {
            Author = null;
            HasAuthor = false;
            Message = message;
            Metadata = metadata;
        }

        public Line(string author, string message, string metadata)
        {
            Author = author;
            HasAuthor = true;
            Message = message;
            Metadata = metadata;
        }
    }

}
