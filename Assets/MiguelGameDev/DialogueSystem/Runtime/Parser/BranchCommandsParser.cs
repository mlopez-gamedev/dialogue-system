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

        public IDialogueCommand[] ParseCommands(string text, int level = 0, params BranchPosition[] branchPositions)
        {
            text = $"\n{text}";
            var baseSplitter = _baseParser.GetBranchSplitter(level);
            var splitterLength = baseSplitter.Length;

            var splitters = new List<string>();
            var currentParser = _baseParser;
            while (currentParser != null)
            {
                if (currentParser.IsEnabled)
                {
                    splitters.Add(baseSplitter + currentParser.StartsWith);
                }
                currentParser = currentParser.NextParser;
            }

            var splitPattern = string.Join("|", splitters.Select(s => Regex.Escape(s)));
            var pattern = $"({splitPattern})(.*?)(?={splitPattern}|$)";
            var matches = Regex.Matches(text, pattern, RegexOptions.Singleline);
            var commands = new List<IDialogueCommand>(matches.Count);

            var index = 0;
            var lastIndex = 0;
            foreach (Match match in matches)
            {
                var lineCommand = text.Substring(match.Index + splitterLength, match.Length - splitterLength);
                lastIndex = match.Index + match.Length;
                var command = _baseParser.Parse(lineCommand, new CommandPath(index, level, branchPositions));
                if (command == null)
                {
                    // for comments
                    continue;
                }

                ++index;
                commands.Add(command);
            }

            // This print invalid lines, so user can write
            var lastLineCommand = text.Substring(lastIndex);
            if (!string.IsNullOrEmpty(lastLineCommand))
            {
                var command = _baseParser.Parse(lastLineCommand, new CommandPath(index, level, branchPositions));
                commands.Add(command);
            }

            return commands.ToArray();
        }
    }
}
