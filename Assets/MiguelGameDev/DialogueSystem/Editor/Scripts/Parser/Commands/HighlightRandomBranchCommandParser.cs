using System;
using System.Linq;
using System.Text.RegularExpressions;
using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.Parser.Command;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class HighlightRandomBranchCommandParser : CommandParser
    {
        private readonly IHighlightRandomBranchCommandFactory _randomBranchCommandFactory;
        private readonly HighlightBranchParser _branchParser;
        
        public override string StartsWith => "rand";
        private const string RandomSplitter = "%";
        
        private readonly string _startWithColor;
        private readonly string _randomChancesStartWithColor;
        private readonly string _randomChancesColor;
        private readonly string _wrongTextColor;
        private readonly string _errorColor;
        
        public HighlightRandomBranchCommandParser(IHighlightRandomBranchCommandFactory lineCommandFactory, HighlightBranchParser branchParser, HighlightStyle style)
        {
            _randomBranchCommandFactory = lineCommandFactory;
            _branchParser = branchParser;

            _startWithColor = "#" + ColorUtility.ToHtmlStringRGB(style.RandomStartColor);
            _randomChancesStartWithColor = "#" + ColorUtility.ToHtmlStringRGB(style.RandomChancesStartColor);
            _randomChancesColor = "#" + ColorUtility.ToHtmlStringRGB(style.RandomChancesColor);
            _wrongTextColor = "#" + ColorUtility.ToHtmlStringRGB(style.WrongTextColor);
            _errorColor = "#" + ColorUtility.ToHtmlStringRGB(style.ErrorColor);
        }
        
        protected override bool TryParse(string lineCommand, CommandPath commandPath, out IDialogueCommand command)
        {
            if (!lineCommand.StartsWith(StartsWith))
            {
                command = null;
                return false;
            }
            
            var chancesAndBranches = lineCommand.Split(GetRandomSplitter(commandPath.Level));

            if (chancesAndBranches.Length < 2)
            {
                command = null;
                return false;
            }
            
            var highlightedChances = GetBranchStarts(commandPath.Level);
            highlightedChances += HighlightRand(chancesAndBranches[0]);

            var branchSelectors = new HighlightSelectBranch[chancesAndBranches.Length - 1];
            for (int i = 1; i < chancesAndBranches.Length; ++i)
            {
                var splits = chancesAndBranches[i].Split(GetBranchSplitter(commandPath.Level + 1));
                //var highlightedSelector = GetBranchStarts(commandPath.Level);
                var highlightedSelector = HighlightSelector(splits[0], commandPath.Level);
                
                IBranch branch = null;
                if (splits.Length > 1)
                {
                    var branchPosition = new BranchPosition(commandPath.CommandIndex, i);
                    var branchText = chancesAndBranches[i].Substring(splits[0].Length);
                    branch = _branchParser.Parse(branchText, commandPath.CommandIndex, commandPath.Level + 1, commandPath.BranchPositions.Append(branchPosition).ToArray());
                }

                branchSelectors[i - 1] = new HighlightSelectBranch(highlightedSelector, branch);
            }

            command = _randomBranchCommandFactory.CreateHighlightCommand(highlightedChances, branchSelectors);
            return true;
        }

        private string HighlightRand(string randCommand)
        {
            string highlightedCommand = $"<b><color={_startWithColor}>{StartsWith}</color></b>";
            randCommand = randCommand.Substring(StartsWith.Length);
            
            if (string.IsNullOrWhiteSpace(randCommand))
            {
                return highlightedCommand;
            }
            
            highlightedCommand += Regex.Unescape($"<i><color={_wrongTextColor}>{randCommand}</color></i> <color={_errorColor}>(this will be ignored)</color>");
            return highlightedCommand;
        }
        
        private string HighlightSelector(string chancesCommand, int level)
        {
            string highlightedCommand = $"<b><color={_randomChancesStartWithColor}>{GetRandomSplitter(level)}</color></b>";

            var highlightedLine = String.Empty;
            if (!float.TryParse(chancesCommand, out float _))
            {
                highlightedLine =  $"<color={_wrongTextColor}>{chancesCommand}</color> <color={_errorColor}>(this should be a number)</color>";
            }
            else
            {
                highlightedLine =  $"<color={_randomChancesColor}>{chancesCommand}</color>";;    
            }
            
            highlightedCommand += highlightedLine;

            return highlightedCommand;
        }
        
        private string GetRandomSplitter(int level)
        {
            var randomSplitter = "\n";
            for (int i = 0; i < level; i++)
            {
                randomSplitter += BranchPrefix;
            }
            return randomSplitter + RandomSplitter;
        }
    }
}