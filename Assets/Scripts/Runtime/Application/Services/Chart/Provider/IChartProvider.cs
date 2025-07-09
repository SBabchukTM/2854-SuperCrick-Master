using Application.Game;
using R3;

namespace Runtime.Application.Services.Chart.Provider
{
    public interface IChartProvider : IReset
    {
        ReactiveProperty<int> PlayerScoreSubject { get; }
        ReactiveProperty<int> EnemyScoreSubject { get; }

        string GetChartScore();

        void SetEnemyScore(int enemyScore);

        void SetPlayerScore(int playerScore);
    }
}