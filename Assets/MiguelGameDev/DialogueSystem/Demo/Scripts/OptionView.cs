using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiguelGameDev.DialogueSystem.Demo
{
    public class OptionView : MonoBehaviour
    {
        [SerializeField] Button _selectButton;
        [SerializeField] TMP_Text _optionText;

        private LineWithOptionsView _view;
        private int _selectIndex;

        public void SetOption(LineWithOptionsView view, SelectBranch option)
        {
            string message = option.Message;
            if (option.HasMetadata)
            {
                message += $" <i><color=grey>({option.Metadata})<color=grey></i>";
            }
            _optionText.text = message;
            
            _view = view;
            _selectIndex = option.BranchIndex;
            _selectButton.onClick.AddListener(SelectOption);
            _selectButton.interactable = option.Metadata != "disabled";
        }

        private void SelectOption()
        {
            _selectButton.onClick.RemoveAllListeners();
            _view.SelectBranch(_selectIndex);
        }
    }
}
