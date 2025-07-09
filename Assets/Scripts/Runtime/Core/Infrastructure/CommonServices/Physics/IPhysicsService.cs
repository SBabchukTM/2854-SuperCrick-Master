using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.Common.Physics
{
    public interface IPhysicsService
    {
        IEnumerable<GameObject> RaycastAll(Vector2 worldPosition, Vector2 direction, int layerMask);

        GameObject Raycast(Vector2 worldPosition, Vector2 direction, int layerMask);

        GameObject LineCast(Vector2 start, Vector2 end, int layerMask);

        IEnumerable<GameObject> CircleCast(Vector3 position, float radius, int layerMask);

        int CircleCastNonAlloc(Vector3 position, float radius, int layerMask, GameObject[] hitBuffer);

        GameObject OverlapPoint(Vector2 worldPosition, int layerMask);

        int OverlapCircle(Vector3 worldPos, float radius, Collider2D[] hits, int layerMask);
    }
}