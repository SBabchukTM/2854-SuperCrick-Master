namespace Application.Game
{
    public interface IGameResultProvider
    {
        void SetGameResult(GameResult gameResult);

        GameResult GetGameResult();

        GameResultConfig GetGameResultConfig();
    }
}