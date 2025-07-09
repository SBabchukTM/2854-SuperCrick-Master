using Core;

namespace Application.Game
{
    public class GameResultProvider : IGameResultProvider
    {
        private readonly ISettingProvider _settingProvider;

        private GameResultConfig _gameResultConfig;

        public GameResultProvider(ISettingProvider settingProvider) =>
                _settingProvider = settingProvider;

        public void SetGameResult(GameResult gameResult) =>
                _gameResultConfig = _settingProvider
                        .Get<GameResultSetup>().GameResultConfigs
                        .Find(x => x.GameResult == gameResult);

        public GameResult GetGameResult() =>
                _gameResultConfig.GameResult;

        public GameResultConfig GetGameResultConfig() =>
                _gameResultConfig;
    }
}