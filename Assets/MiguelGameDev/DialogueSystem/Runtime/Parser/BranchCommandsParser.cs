using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.Parser.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Parser
{
    public class BranchCommandsParser : IBranchCommandsParserStrategy
    {
        private readonly CommandParser _baseParser;

        public BranchCommandsParser(CommandParser baseParser)
        {
            _baseParser = baseParser;
        }

        public IDialogueCommand[] ParseCommands(string text, int tabs = 0, params BranchPosition[] branchPositions)
        {
            text = $"\n{text}";
            var baseSplitter = "\n";
            for (int i = 0; i < tabs; ++i)
            {
                baseSplitter += "\t";
            }

            var splitterLength = baseSplitter.Length;

            var splitters = new List<string>();
            var currentParser = _baseParser;
            while (currentParser != null)
            {
                if (!string.IsNullOrEmpty(currentParser.StartsWith))
                {
                    splitters.Add( baseSplitter + currentParser.StartsWith);
                }
                currentParser = currentParser.NextParser;
            }

            var splitPattern = string.Join("|", splitters.Select(s => Regex.Escape(s)));
            var pattern = $"({splitPattern})(.*?)(?={splitPattern}|$)";

            var matches = Regex.Matches(text, pattern, RegexOptions.Singleline);
            var commands = new List<IDialogueCommand>(matches.Count);

            var index = 0;
            foreach (Match match in matches)
            {
                var lineCommand = text.Substring(match.Index + splitterLength, match.Length - splitterLength);
                var command = _baseParser.Parse(lineCommand, new CommandPath(index, tabs, branchPositions));
                if (command == null)
                {
                    // for comments
                    continue;
                }

                ++index;
                commands.Add(command);
            }

            return commands.ToArray();
        }
    }
}
