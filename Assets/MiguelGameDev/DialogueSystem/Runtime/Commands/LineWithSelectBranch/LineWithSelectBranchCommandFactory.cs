using MiguelGameDev.DialogueSystem.Parser.Command;
using MiguelGameDev.DialogueSystem.UseCases;

namespace MiguelGameDev.DialogueSystem.Commands
{
    public class LineWithSelectBranchCommandFactory : ILineWithSelectBranchCommandFactory
    {
        private readonly IShowLineWithOptionsUseCase _showLineWithOptionsUseCase;

        public LineWithSelectBranchCommandFactory(IShowLineWithOptionsUseCase _showLineWithOptionsUseCase)
        {
            this._showLineWithOptionsUseCase = _showLineWithOptionsUseCase;
        }

        public IDialogueCommand CreateLineWithSelectBranchCommand(Line line, SelectBranchInfo[] selectBranchInfos)
        {
            return new LineWithSelectBranchCommand(_showLineWithOptionsUseCase, line, selectBranchInfos);
        }
    }

}
