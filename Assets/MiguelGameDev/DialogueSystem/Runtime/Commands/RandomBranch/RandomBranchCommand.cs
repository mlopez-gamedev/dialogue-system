using System.Collections.Generic;
using MiguelGameDev.DialogueSystem.Parser.Command;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Commands
{
    public class RandomBranchCommand : IDialogueCommand
    {
        private float _totalChances = 0;
        private readonly Dictionary<RandomBranchInfo, float> _randomBranchInfos;

        private IDialogue _dialogue;
        private SelectBranch[] _selectBranches;

        public RandomBranchCommand(RandomBranchInfo[] randomBranchInfos)
        {
            _randomBranchInfos = new Dictionary<RandomBranchInfo, float>(randomBranchInfos.Length);
            foreach (var randomBranchInfo in randomBranchInfos)
            {
                if (randomBranchInfo.Chances <= 0)
                {
                    continue;
                }
                _totalChances += randomBranchInfo.Chances;
                _randomBranchInfos.Add(randomBranchInfo, randomBranchInfo.Chances);
            }
        }

        public void CreateBranches(IBranch branch)
        {
            foreach (RandomBranchInfo randomBranchInfo in _randomBranchInfos.Keys)
            {
                if (randomBranchInfo.ContinueInCurrentBranch)
                {
                    continue;
                }
                branch.RegisterBranch(randomBranchInfo.BranchPosition, randomBranchInfo.Branch);
                randomBranchInfo.Branch.CreateBranches();
            }
        }

        public void Setup(IDialogue dialogue, IBranch _)
        {
            _dialogue = dialogue;
        }

        public void Execute()
        {
            float random = Random.Range(0f, _totalChances);
            float randomPosition = _totalChances;
            foreach (KeyValuePair<RandomBranchInfo, float> randomBranchInfo in _randomBranchInfos)
            {
                randomPosition -= randomBranchInfo.Value;
                if (random < randomPosition)
                {
                    continue;
                }
                
                _dialogue.SelectBranch(randomBranchInfo.Key.BranchPosition.BranchIndex);
                break;
            }
        }
    }
}