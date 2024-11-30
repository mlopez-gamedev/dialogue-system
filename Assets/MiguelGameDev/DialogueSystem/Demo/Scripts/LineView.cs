using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiguelGameDev.DialogueSystem.Demo
{
    public class LineView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _authorText;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Button _nextButton;

        private IDialogue _dialogue;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Setup(IDialogue dialogue)
        {
            _dialogue = dialogue;
        }

        public void SetLine(Line line)
        {
            gameObject.SetActive(true);
            _nextButton.onClick.AddListener(Next);
            _messageText.text = line.Message;
            if (!line.HasAuthor)
            {
                _authorText.gameObject.SetActive(false);
                return;
            }

            _authorText.gameObject.SetActive(true);
            _authorText.text = line.Author;
        }

        public void Next()
        {
            gameObject.SetActive(false);
            _nextButton.onClick.RemoveAllListeners();
            _dialogue.Next();
        }
    }
}
