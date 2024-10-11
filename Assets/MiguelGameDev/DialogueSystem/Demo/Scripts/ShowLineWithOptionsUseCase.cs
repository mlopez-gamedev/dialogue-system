using MiguelGameDev.DialogueSystem.UseCases;

namespace MiguelGameDev.DialogueSystem.Demo
{
    public class ShowLineWithOptionsUseCase : IShowLineWithSelectBranchUseCase
    {
        private readonly LineWithOptionsView _lineWithOptionsView;

        public ShowLineWithOptionsUseCase(LineWithOptionsView lineWithOptionsView)
        {
            _lineWithOptionsView = lineWithOptionsView;
        }

        public void ShowLineWithSelectBranch(Line line, SelectBranch[] selectBranches)
        {
            _lineWithOptionsView.SetLine(line, selectBranches);
        }
    }
}
