using MiguelGameDev.DialogueSystem.Commands;
using MiguelGameDev.DialogueSystem.Parser.Command;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiguelGameDev.DialogueSystem.Demo
{
    public class DemoInstaller : MonoBehaviour
    {
        [SerializeField] private DialogueText _dialogueText;
        [SerializeField] private LineView _lineView;
        [SerializeField] private LineWithOptionsView _lineWithOptionsView;

        // Start is called before the first frame update
        void Start()
        {
            var showLineUseCase = new ShowLineUseCase(_lineView);
            var showLineWithOptionsUseCase = new ShowLineWithOptionsUseCase(_lineWithOptionsView);

            var dialogueContext = new DialogueContext();
            var dialogueInstaller = new DialogueParserInstaller(dialogueContext, showLineUseCase, showLineWithOptionsUseCase);
            var dialogueParser = dialogueInstaller.Install();
            var dialogue = dialogueParser.Parse(_dialogueText.Text);
            dialogue.Setup();

            dialogueContext.Setup(dialogue);
            _lineView.Setup(dialogue);
            _lineWithOptionsView.Setup(dialogue);


            dialogue.OnDialogueEnd += Dialogue_OnDialogueEnd;
            dialogue.Start();
        }

        private void Dialogue_OnDialogueEnd()
        {
            Debug.Log("Dialogue End");
        }
    }
}
