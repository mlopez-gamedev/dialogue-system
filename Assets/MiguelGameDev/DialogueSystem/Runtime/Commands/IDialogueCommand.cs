using System.Drawing;

namespace MiguelGameDev.DialogueSystem.Commands
{
    public interface IDialogueCommand
    {
        void CreateBranches(IBranch branch);
        void Setup(IDialogue dialogue, IBranch branch);
        void Execute();
    }

}
