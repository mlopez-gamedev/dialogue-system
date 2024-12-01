using MiguelGameDev.DialogueSystem.Parser.Command;
using MiguelGameDev.DialogueSystem.UseCases;

namespace MiguelGameDev.DialogueSystem.Commands
{
    public class LineWithSelectBranchCommand : IDialogueCommand
    {
        private readonly IShowLineWithOptionsUseCase _showLineWithOptionsUseCase;
        private readonly Line _line;
        private readonly SelectBranchInfo[] _selectBranchInfos;

        private SelectBranch[] _selectBranches;

        public LineWithSelectBranchCommand(IShowLineWithOptionsUseCase showLineUseCase, Line line, SelectBranchInfo[] selectBranchInfos)
        {
            _showLineWithOptionsUseCase = showLineUseCase;
            _line = line;
            _selectBranchInfos = selectBranchInfos;
        }

        public void CreateBranches(IBranch branch)
        {
            _selectBranches = new SelectBranch[_selectBranchInfos.Length];
            for (int i = 0; i < _selectBranchInfos.Length; ++i)
            {
                _selectBranches[i] = new SelectBranch(_selectBranchInfos[i].Text, _selectBranchInfos[i].Metadata, _selectBranchInfos[i].BranchPosition.BranchIndex);
                UnityEngine.Debug.Log($"{_selectBranchInfos[i].Text} => {_selectBranchInfos[i].BranchPosition.BranchIndex}");
                if (_selectBranchInfos[i].ContinueInCurrentBranch)
                {
                    continue;
                }
                branch.RegisterBranch(_selectBranchInfos[i].BranchPosition, _selectBranchInfos[i].Branch);
                _selectBranchInfos[i].Branch.CreateBranches();
            }
        }

        public void Setup(IDialogue _, IBranch __)
        {
            // Nothing to do here
        }

        public void Execute()
        {
            _showLineWithOptionsUseCase.ShowLineWithOptions(_line, _selectBranches);
        }
    }
}
