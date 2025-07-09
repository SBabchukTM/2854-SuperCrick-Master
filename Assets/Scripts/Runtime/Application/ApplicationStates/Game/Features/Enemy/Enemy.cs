using UnityEngine;

namespace Runtime.Application.ApplicationStates.Game.Features.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform _ballSpawnPosition;

        public Transform BallSpawnPosition => _ballSpawnPosition;

        public float TimeToCreateBall;
    }
}