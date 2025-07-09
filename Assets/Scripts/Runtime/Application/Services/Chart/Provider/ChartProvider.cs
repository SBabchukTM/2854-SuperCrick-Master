using R3;

namespace Runtime.Application.Services.Chart.Provider
{
    public class ChartProvider : IChartProvider
    {
        public ReactiveProperty<int> PlayerScoreSubject { get; } = new();
        public ReactiveProperty<int> EnemyScoreSubject { get; } = new();

        public string GetChartScore() =>
                $"{PlayerScoreSubject} : {EnemyScoreSubject}";

        public void SetPlayerScore(int playerScore) =>
                PlayerScoreSubject.OnNext(playerScore);

        public void SetEnemyScore(int enemyScore) =>
                EnemyScoreSubject.OnNext(enemyScore);
        
        public void Reset()
        {
            PlayerScoreSubject.OnNext(0);
            EnemyScoreSubject.OnNext(0);
        }
    }
}