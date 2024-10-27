namespace MiguelGameDev.DialogueSystem.Commands
{
    public class DialogueContext
    {
        private IDialogue _dialogue;

        public DialogueContext()
        {

        }

        public void Setup(IDialogue dialogue)
        {
            _dialogue = dialogue;
        }

        public void DoSomething(bool boolParam, float numericParam, string stringParam)
        {
            UnityEngine.Debug.Log($"Call DoSomething(boolParam: {boolParam}, numericParam: {numericParam}, stringParam: {stringParam})");
            _dialogue.Next();
        }
    }
}
