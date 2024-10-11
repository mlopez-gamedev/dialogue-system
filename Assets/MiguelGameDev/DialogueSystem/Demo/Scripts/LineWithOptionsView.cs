using TMPro;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Demo
{
    public class LineWithOptionsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _authorText;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private OptionView _optionViewPrefab;
        [SerializeField] private Transform _optionsContainer;

        private IDialogue _dialogue;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Setup(IDialogue dialogue)
        {
            _dialogue = dialogue;
        }

        public void SetLine(Line line, SelectBranch[] selectBranches)
        {
            foreach (var branch in selectBranches)
            {
                Instantiate(_optionViewPrefab, _optionsContainer)
                    .SetOption(this, branch);
            }
            gameObject.SetActive(true);

            _messageText.text = line.Text;
            if (!line.HasAuthor)
            {
                _authorText.gameObject.SetActive(false);
                return;
            }

            _authorText.gameObject.SetActive(true);
            _authorText.text += line.Author;
        }

        public void SelectBranch(int branchIndex)
        {
            gameObject.SetActive(false);

            for (int i = 0; i < _optionsContainer.childCount; i++)
            {
                Destroy(_optionsContainer.GetChild(i).gameObject);
            }

            _dialogue.SelectBranch(branchIndex);
        }
    }
}
