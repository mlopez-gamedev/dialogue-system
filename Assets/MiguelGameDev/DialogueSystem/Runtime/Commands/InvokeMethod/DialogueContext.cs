namespace MiguelGameDev.DialogueSystem.Commands
{
    public abstract class DialogueContext
    {
        protected IDialogue _dialogue;

        public void Setup(IDialogue dialogue)
        {
            _dialogue = dialogue;
        }
    }
}
