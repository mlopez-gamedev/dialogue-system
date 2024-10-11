using MiguelGameDev.DialogueSystem.Parser.Command;

namespace MiguelGameDev.DialogueSystem.UseCases
{
    public interface IShowLineWithSelectBranchUseCase
    {
        void ShowLineWithSelectBranch(Line line, SelectBranch[] selectBranches);
    }
}
