using UnityEngine;

namespace Application.Game
{
    [CreateAssetMenu(fileName = "GameResultConfig", menuName = "Config/GameResult/GameResultConfig")]
    public sealed class GameResultConfig : ScriptableObject
    {
        public GameResult GameResult;
        public Sprite Sprite;
        public string Message;
    }
}