using UnityEditor;
using MiguelGameDev.DialogueSystem.Parser.Command;
using MiguelGameDev.DialogueSystem.Parser;
using System.Text;

namespace MiguelGameDev.DialogueSystem.Editor
{
    public class TextHighlighter
    {
        private const string HighlightStylePath = "Assets/MiguelGameDev/DialogueSystem/Editor/Config/HighlightStyle.asset";

        private HighlightStyle _style;
        private StringBuilder _stringBuilder;
        private IHighlightCommandFactory _highlightCommandFactory;
        private IHighlightLineWithSelectBranchCommandFactory _highlightSelectLineCommandFactory;
        private IHighlightTitleCommandFactory _highlightTitleCommandFactory;
        private IHighlightGoToCommandFactory _highlightGoToCommandFactory;
        private IHighlightRandomBranchCommandFactory _highlightRandomBranchCommandFactory;

        private HighlightDialogueParser _dialogueParser;

        public HighlightStyle Style => _style;

        public TextHighlighter()
        {
            _style = AssetDatabase.LoadAssetAtPath<HighlightStyle>(HighlightStylePath);
            _stringBuilder = new StringBuilder();

            _highlightCommandFactory = new HighlightCommandFactory(_stringBuilder);
            _highlightSelectLineCommandFactory = new HighlightLineWithSelectBranchCommandFactory(_stringBuilder);
            _highlightTitleCommandFactory = new HighlightTitleCommandFactory(_stringBuilder, _style);
            _highlightGoToCommandFactory = new HighlightGoToCommandFactory(_stringBuilder, _style);
            _highlightRandomBranchCommandFactory = new HighlightRandomBranchCommandFactory(_stringBuilder);

            var branchParser = new HighlightBranchParser();
            var baseParser = CreateParserChainOfResponsability(branchParser);
            var parseStrategy = new BranchCommandsParser(baseParser);
            branchParser.Setup(parseStrategy);
            _dialogueParser = new HighlightDialogueParser(branchParser);
        }

        public string Highlight(string text)
        {
            var dialogue = _dialogueParser.Parse(text);

            dialogue.Setup();
            dialogue.Start();

            var highlightedText = _stringBuilder.ToString();
            _stringBuilder.Clear();
            return highlightedText;
        }

        private CommandParser CreateParserChainOfResponsability(HighlightBranchParser branchParser)
        {
            var selectionLineCommandParser = new HighlightLineWithSelectBranchCommandParser(_highlightSelectLineCommandFactory, branchParser, _style);
            var lineCommandParser = new HighlightLineCommandParser(_highlightCommandFactory, _style);
            var titleCommandParser = new HighlightTitleParser(_highlightTitleCommandFactory, _style);
            var goToCommandParser = new HighlightGoToCommandParser(_highlightGoToCommandFactory, _style);
            var invokeMethodCommandParser = new HighlightInvokeMethodCommandParser(_highlightCommandFactory, _style);
            var randomBranchCommandParser = new HighlightRandomBranchCommandParser(_highlightRandomBranchCommandFactory, branchParser, _style);
            var commentParser = new HighlightCommentParser(_highlightCommandFactory, _style);
            var defaultParser = new DefaultHighlightParser(_highlightCommandFactory, _style);
            var notFoundParser = new NotFoundHighlightParser(_highlightCommandFactory, _style);

            selectionLineCommandParser.SetNextParser(lineCommandParser);
            lineCommandParser.SetNextParser(titleCommandParser);
            titleCommandParser.SetNextParser(goToCommandParser);
            goToCommandParser.SetNextParser(invokeMethodCommandParser);
            invokeMethodCommandParser.SetNextParser(randomBranchCommandParser);
            randomBranchCommandParser.SetNextParser(commentParser);
            commentParser.SetNextParser(defaultParser);
            defaultParser.SetNextParser(notFoundParser);

            return selectionLineCommandParser;
        }
    }
}
