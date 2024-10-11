using MiguelGameDev.DialogueSystem.Parser.Command;
using MiguelGameDev.DialogueSystem.UseCases;

namespace MiguelGameDev.DialogueSystem.Commands
{
    public class LineWithSelectBranchCommandFactory : ILineWithSelectBranchCommandFactory
    {
        private readonly IShowLineWithSelectBranchUseCase _showLineWithSelectBranchUseCase;

        public LineWithSelectBranchCommandFactory(IShowLineWithSelectBranchUseCase showLineWithSelectBranchUseCase)
        {
            _showLineWithSelectBranchUseCase = showLineWithSelectBranchUseCase;
        }

        public IDialogueCommand CreateLineWithSelectBranchCommand(Line line, SelectBranchInfo[] selectBranchInfos)
        {
            return new LineWithSelectBranchCommand(_showLineWithSelectBranchUseCase, line, selectBranchInfos);
        }
    }

}
