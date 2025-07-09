using System.Collections.Generic;
using UnityEngine;

namespace Application.Game
{
    public class LevelPointProvider : ILevelPointProvider
    {
        private readonly Dictionary<LevelPointTypeId, Vector2> _levelPoints = new();

        public void AddPoint(LevelPointTypeId levelPointTypeId, Vector2 point) =>
                _levelPoints.Add(levelPointTypeId, point);
        
        public void RemovePoint(LevelPointTypeId levelPointTypeId) =>
            _levelPoints.Remove(levelPointTypeId);
        
        public Vector2 GetPoint(LevelPointTypeId levelPointTypeId) =>
                _levelPoints[levelPointTypeId];
    }
}