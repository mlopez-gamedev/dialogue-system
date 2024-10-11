using UnityEngine;

namespace MiguelGameDev.DialogueSystem
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "MiguelGameDev/Dialogue")]
    public class DialogueText : ScriptableObject
    {
        [SerializeField, TextArea(20, 50)] private string _text;

        public string Text => _text;
    }
}