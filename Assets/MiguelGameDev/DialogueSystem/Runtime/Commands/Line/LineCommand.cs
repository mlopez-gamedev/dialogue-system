using MiguelGameDev.DialogueSystem.UseCases;

namespace MiguelGameDev.DialogueSystem.Commands
{
    public class LineCommand : IDialogueCommand
    {
        private readonly IShowLineUseCase _showLineUseCase;
        private readonly Line _line;

        public LineCommand(IShowLineUseCase showLineUseCase, Line line)
        {
            _showLineUseCase = showLineUseCase;
            _line = line;
        }

        public void CreateBranches(IBranch _)
        {
            // Nothing to do here
        }

        public void Setup(IDialogue _, IBranch __)
        {
            // Nothing to do here
        }

        public void Execute()
        {
            _showLineUseCase.ShowLine(_line);
        }
    }
}
