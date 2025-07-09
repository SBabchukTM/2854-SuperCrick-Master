using System.Collections.Generic;
using UnityEngine;

namespace Code.Gameplay.Common.Physics
{
    public class PhysicsService : IPhysicsService
    {
        private readonly RaycastHit2D[] _hits = new RaycastHit2D[128];
        private readonly Collider2D[] _overlapHits = new Collider2D[128];

        public IEnumerable<GameObject> RaycastAll(Vector2 worldPosition, Vector2 direction, int layerMask)
        {
            var hitCount = Physics2D.RaycastNonAlloc(worldPosition, direction, _hits, float.MaxValue, layerMask);

            for (var i = 0; i < hitCount; i++)
            {
                var hit = _hits[i];

                if(hit.collider == null)
                    continue;

                yield return hit.collider.gameObject;
            }
        }

        public GameObject Raycast(Vector2 worldPosition, Vector2 direction, int layerMask)
        {
            var hitCount = Physics2D.RaycastNonAlloc(worldPosition, direction, _hits, float.MaxValue, layerMask);

            for (var i = 0; i < hitCount; i++)
            {
                var hit = _hits[i];

                if(hit.collider == null)
                    continue;

                return hit.collider.gameObject;
            }

            return null;
        }

        public GameObject LineCast(Vector2 start, Vector2 end, int layerMask)
        {
            var direction = end - start;
            var distance = direction.magnitude;
            direction.Normalize();

            var hitCount = Physics2D.RaycastNonAlloc(start, direction, _hits, distance, layerMask);

            for (var i = 0; i < hitCount; i++)
            {
                var hit = _hits[i];

                if(hit.collider == null)
                    continue;

                return hit.collider.gameObject;
            }

            return null;
        }

        public IEnumerable<GameObject> CircleCast(Vector3 position, float radius, int layerMask)
        {
            var hitCount = Physics2D.OverlapCircleNonAlloc(position, radius, _overlapHits, layerMask);

            DrawDebugCircle(position, radius, 32, Color.red, 1f);

            for (var i = 0; i < hitCount; i++)
            {
                if(_overlapHits[i] == null)
                    continue;

                yield return _overlapHits[i].gameObject;
            }
        }

        public int CircleCastNonAlloc(Vector3 position, float radius, int layerMask, GameObject[] hitBuffer)
        {
            var hitCount = Physics2D.OverlapCircleNonAlloc(position, radius, _overlapHits, layerMask);

            DrawDebugCircle(position, radius, 32, Color.green, 1f);

            for (var i = 0; i < hitCount; i++)
            {
                if(_overlapHits[i] == null)
                    continue;

                if(i < hitBuffer.Length)
                    hitBuffer[i] = _overlapHits[i].gameObject;
            }

            return hitCount;
        }

        public GameObject OverlapPoint(Vector2 worldPosition, int layerMask)
        {
            var hitCount = Physics2D.OverlapPointNonAlloc(worldPosition, _overlapHits, layerMask);

            for (var i = 0; i < hitCount; i++)
            {
                var hit = _overlapHits[i];

                if(hit == null)
                    continue;

                return hit.gameObject;
            }

            return null;
        }

        public int OverlapCircle(Vector3 worldPos, float radius, Collider2D[] hits, int layerMask) => Physics2D.OverlapCircleNonAlloc
                (worldPos, radius, hits, layerMask);

        private static void DrawDebugCircle(Vector3 center, float radius, int segments, Color color, float duration)
        {
            for (var i = 0; i < segments; i++)
            {
                var angle = i * (360f / segments);
                var start = center + Quaternion.Euler(0, 0, angle) * Vector3.right * radius;
                var end = center + Quaternion.Euler(0, 0, angle + 1) * Vector3.right * radius;
                Debug.DrawLine(start, end, color, duration);
            }
        }
    }
}