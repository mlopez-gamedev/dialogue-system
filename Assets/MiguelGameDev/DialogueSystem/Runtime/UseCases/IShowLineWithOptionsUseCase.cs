using MiguelGameDev.DialogueSystem.Parser.Command;

namespace MiguelGameDev.DialogueSystem.UseCases
{
    public interface IShowLineWithOptionsUseCase
    {
        void ShowLineWithOptions(Line line, SelectBranch[] selectBranches);
    }
}
