using UnityEngine;

namespace Code.Gameplay.Common.Random
{
    public interface IRandomService
    {
        float Range(float inclusiveMin, float inclusiveMax);

        int Range(int inclusiveMin, int exclusiveMax);

        Vector2 GetRandomDirection(Vector2 position);
    }
}