using UnityEngine;

namespace Application.Game
{
    public interface ILevelPointProvider
    {
        void AddPoint(LevelPointTypeId levelPointTypeId, Vector2 point);

        void RemovePoint(LevelPointTypeId levelPointTypeId);

        Vector2 GetPoint(LevelPointTypeId levelPointTypeId);
    }
}