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

        public void CreateBranches(IBranch ___)
        {
            // Nothing to do here
        }

        public void Setup(IDialogue __, IBranch ___)
        {
            // Nothing to do here
        }

        public void Execute()
        {
            _showLineUseCase.ShowLine(_line);
        }
    }
}
