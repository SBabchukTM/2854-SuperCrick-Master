using System.Collections.Generic;
using Application.UI;
using Core.Factory;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Application.Game
{
    public class ChooseTeamScreen : ClosableScreen
    {
        private readonly List<ChooseTeamButton> _teamButtons = new();

        public readonly Subject<Unit> PlayButtonPressedSubject = new();

        [SerializeField] private SimpleButton _playButton;
        [SerializeField] private Transform _parent;

        private GameObjectFactory _gameObjectFactory;
        private ChooseTeamButton _enemyTeam;
        private ChooseTeamButton _choosedTeam;
        private IChoosedTeamProvider _choosedTeamProvider;

        [Inject]
        public void Construct(GameObjectFactory gameObjectFactory, IChoosedTeamProvider choosedTeamProvider)
        {
            _choosedTeamProvider = choosedTeamProvider;
            _gameObjectFactory = gameObjectFactory;
        }

        public void CreateTeamButtons(GameObject view, params Sprite[] sprites)
        {
            foreach (var sprite in sprites)
            {
                var teamButton = Create(view, sprite);
                Subscribe(teamButton);
                _teamButtons.Add(teamButton.GetComponent<ChooseTeamButton>());
            }

            SetMark(_teamButtons[0]);
            _enemyTeam = _teamButtons[1];
        }

        private void Subscribe(ChooseTeamButton teamButton)
        {
            teamButton.Button
                    .OnClickAsObservable()
                    .Subscribe(_ => SetMark(teamButton))
                    .AddTo(teamButton);
            
            _playButton.Button
                    .OnClickAsObservable()
                    .Subscribe(_ => OnPlayButtonPressed())
                    .AddTo(this);
        }

        private ChooseTeamButton Create(GameObject view, Sprite sprite)
        {
            var teamButton = _gameObjectFactory.Create(view, _parent);
            teamButton.GetComponentInChildren<Image>().sprite = sprite;

            return teamButton.GetComponentInChildren<ChooseTeamButton>();
        }

        private void SetMark(ChooseTeamButton chooseTeamButton)
        {
            if(_choosedTeam)
            {
                _enemyTeam = _choosedTeam;
                _enemyTeam.EnableMark(false);
            }

            _choosedTeam = chooseTeamButton;
            _choosedTeam.EnableMark(true);
        }

        private void AddTeams(Sprite choosedTeamSprite, Sprite previousTeamSprite)
        {
            var playerTeam = new ChoosedTeamModel { TeamTypeId = TeamTypeId.Player, TeamSprite = choosedTeamSprite };
            var enemyTeam = new ChoosedTeamModel { TeamTypeId = TeamTypeId.Enemy, TeamSprite = previousTeamSprite };
            
            _choosedTeamProvider.AddTeam(playerTeam);
            _choosedTeamProvider.AddTeam(enemyTeam);
        }

        private void OnPlayButtonPressed()
        {
            var choosedTeamSprite = _choosedTeam.GetComponent<Image>().sprite;
            var enemySprite = _enemyTeam.GetComponent<Image>().sprite;
            
            AddTeams(choosedTeamSprite, enemySprite);
            PlayButtonPressedSubject.OnNext(Unit.Default);
        }
    }
}