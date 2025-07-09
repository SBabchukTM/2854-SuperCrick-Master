using System;

namespace Core
{
    [Serializable]
    public struct V2I : IEquatable<V2I>
    {
        public int X;

        public int Y;

        public V2I(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(V2I other) =>
                X == other.X && Y == other.Y;

        public override bool Equals(object obj) =>
                obj is V2I other && Equals(other);

        public override int GetHashCode() =>
                HashCode.Combine(X, Y);

        public static V2I operator +(V2I obj1, V2I obj2)
        {
            return new(obj1.X + obj2.X, obj1.Y + obj2.Y);
        }

        public static bool operator ==(V2I obj1, V2I obj2)
        {
            return obj1.X == obj2.X && obj1.Y == obj2.Y;
        }

        public static bool operator !=(V2I obj1, V2I obj2)
        {
            return obj1.X != obj2.X || obj1.Y != obj2.Y;
        }
    }
}