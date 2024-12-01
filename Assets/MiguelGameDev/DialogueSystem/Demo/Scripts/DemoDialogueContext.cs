using MiguelGameDev.DialogueSystem.Commands;

namespace MiguelGameDev.DialogueSystem.Demo
{
    public class DemoDialogueContext : DialogueContext
    {
        public void DoSomething(bool boolParam, float numericParam, string stringParam)
        {
            UnityEngine.Debug.Log($"Call DoSomething(boolParam: {boolParam}, numericParam: {numericParam}, stringParam: {stringParam})");
            _dialogue.Next();
        }
    }
}