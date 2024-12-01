using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.UseCases;

namespace MiguelGameDev.DialogueSystem.Parser.Command
{
    public class DialogueParserInstaller
    {
        private ILineCommandFactory _lineCommandFactory;
        private ILineWithSelectBranchCommandFactory _lineWithSelectBranchCommandFactory;
        private IInvokeMethodCommandFactory _invokeMethodCommandFactory;
        private IRandomBranchCommandFactory _randomBranchCommandFactory;

        public DialogueParserInstaller(
                DialogueContext dialogueContext,
                IShowLineUseCase showLineUseCase,
                IShowLineWithOptionsUseCase showLineWithOptionsUseCase)
        {
            _lineCommandFactory = new LineCommandFactory(showLineUseCase);
            _lineWithSelectBranchCommandFactory = new LineWithSelectBranchCommandFactory(showLineWithOptionsUseCase);
            _invokeMethodCommandFactory = new InvokeMethodCommandFactory(dialogueContext);
            _randomBranchCommandFactory = new RandomBranchCommandFactory();
        }

        public DialogueParser Install()
        {
            var branchParser = new BranchParser();
            var baseParser = CreateParserChainOfResponsibility(branchParser);
            var commandBranchParserStrategy = new BranchCommandsParser(baseParser);
            branchParser.Setup(commandBranchParserStrategy);
            return new DialogueParser(branchParser);
        }

        private CommandParser CreateParserChainOfResponsibility(BranchParser branchParser)
        {
            var selectionLineCommandParser = new LineWithSelectBranchCommandParser(_lineWithSelectBranchCommandFactory, branchParser);
            var lineCommandParser = new LineCommandParser(_lineCommandFactory);
            var titleCommandParser = new TitleCommandParser();
            var goToCommandParser = new GoToCommandParser();
            var invokeMethodCommandParser = new InvokeMethodCommandParser(_invokeMethodCommandFactory);
            var randomBranchCommandParser = new RandomBranchCommandParser(_randomBranchCommandFactory, branchParser);
            var commentParser = new CommentParser();
            var notFoundParser = new NotFoundParser();

            selectionLineCommandParser.SetNextParser(lineCommandParser);
            lineCommandParser.SetNextParser(titleCommandParser);
            titleCommandParser.SetNextParser(goToCommandParser);
            goToCommandParser.SetNextParser(randomBranchCommandParser);
            randomBranchCommandParser.SetNextParser(invokeMethodCommandParser);
            invokeMethodCommandParser.SetNextParser(commentParser);
            commentParser.SetNextParser(notFoundParser);

            return selectionLineCommandParser;
        }
    }

}
