
namespace SnakeGame
{
    public interface IScoreInfoManager
    {
        public void InitializeScoreBoard();
        public void AddPoints(int points);
        public void ShowAction(string action);
    }
}