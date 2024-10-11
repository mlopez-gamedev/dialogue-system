using MiguelGameDev.DialogueSystem.UseCases;

namespace MiguelGameDev.DialogueSystem.Demo
{
    public class ShowLineUseCase : IShowLineUseCase
    {
        private readonly LineView _lineView;

        public ShowLineUseCase(LineView lineView)
        {
            _lineView = lineView;
        }

        public void ShowLine(Line line)
        {
            _lineView.SetLine(line);
        }
    }
}
