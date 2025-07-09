using Application.Game;
using R3;
using Runtime.Application.Services.Chart.Provider;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.Application.Services.Chart
{
    public class Chart : MonoBehaviour
    { 
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Transform _firstTeamContainer;
        [SerializeField] private Transform _secondTeamContainer;

        private IChoosedTeamProvider _choosedTeamProvider;
        private IChartProvider _chartProvider;

        [Inject]
        public void Construct(IChoosedTeamProvider choosedTeamProvider, IChartProvider chartProvider)
        {
            _chartProvider = chartProvider;
            _choosedTeamProvider = choosedTeamProvider;
        }

        private void Start()
        {
            AddTeam(_choosedTeamProvider.GetTeam(TeamTypeId.Player));
            AddTeam(_choosedTeamProvider.GetTeam(TeamTypeId.Enemy));

            Subscribe();
            UpdateScore();
        }

        private void Subscribe() =>
                _choosedTeamProvider.Teams.OnUpdate
                        .Subscribe(_ => UpdateScore())
                        .AddTo(this);

        private void AddTeam(ChoosedTeamModel teamKey)
        {
            var newTeam = new GameObject("Team");
            newTeam.AddComponent<SpriteRenderer>().sprite = teamKey.TeamSprite;
            newTeam.transform.SetParent(GetContainerThroughId(teamKey)); 
            newTeam.transform.localPosition = Vector3.zero;
            newTeam.transform.localScale = Vector3.one;
        }

        private void UpdateScore()
        {
            var playerScore = _choosedTeamProvider.GetScoreThroughId(TeamTypeId.Player);
            var enemyScore = _choosedTeamProvider.GetScoreThroughId(TeamTypeId.Enemy);

            _chartProvider.SetPlayerScore(playerScore);
            _chartProvider.SetEnemyScore(enemyScore);
            
            _scoreText.text = $"{playerScore} : {enemyScore}";
        }

        private Transform GetContainerThroughId(ChoosedTeamModel teamKey) =>
                teamKey.TeamTypeId == TeamTypeId.Player ? _firstTeamContainer : _secondTeamContainer;
    }
}