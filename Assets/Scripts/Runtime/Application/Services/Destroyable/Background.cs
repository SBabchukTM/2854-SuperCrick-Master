using UnityEngine;

namespace Application.Game
{
    public class Background : Destroyable
    {
        [SerializeField] private Transform _enemyPosition;

        private ILevelPointProvider _levelPointProvider;

        protected override void OnDestroy()
        {
            _levelPointProvider.RemovePoint(LevelPointTypeId.EnemyPosition);
            
            base.OnDestroy();
        }

        public void SetLevelPointerProvider(ILevelPointProvider levelPointProvider)
        {
            _levelPointProvider = levelPointProvider;
            _levelPointProvider.AddPoint(LevelPointTypeId.EnemyPosition, _enemyPosition.position);
        }
    }
}