using MiguelGameDev.DialogueSystem.UseCases;

namespace MiguelGameDev.DialogueSystem.Commands
{

    public class LineCommandFactory : ILineCommandFactory
    {
        private readonly IShowLineUseCase _showLineUseCase;

        public LineCommandFactory(IShowLineUseCase showLineUseCase)
        {
            _showLineUseCase = showLineUseCase;
        }

        public IDialogueCommand CreateLineCommand(Line line)
        {
            return new LineCommand(_showLineUseCase, line);
        }
    }

}
