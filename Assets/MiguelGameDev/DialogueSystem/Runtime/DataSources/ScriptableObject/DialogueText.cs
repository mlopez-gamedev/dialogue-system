using UnityEngine;

namespace MiguelGameDev.DialogueSystem
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "MiguelGameDev/Dialogue")]
    public class DialogueText : ScriptableObject
    {
        [SerializeField] private string _text = "~ Start";

        public string Text => _text;
    }
}