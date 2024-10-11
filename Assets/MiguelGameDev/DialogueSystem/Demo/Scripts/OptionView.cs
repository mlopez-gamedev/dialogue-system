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
            _optionText.text = option.Text;

            _view = view;
            _selectIndex = option.BranchIndex;

            _selectButton.onClick.AddListener(SelectOption);
        }

        private void SelectOption()
        {
            _selectButton.onClick.RemoveAllListeners();
            _view.SelectBranch(_selectIndex);
        }
    }
}
