using System;
using UnityEngine;

namespace Code.Common.Extensions
{
    public static class CollisionExtensions
    {
        public static bool Matches(this Collider2D collider, LayerMask layerMask)
            => ((1 << collider.gameObject.layer) & layerMask) != 0;

        public static int AsMask<T>(this T layer) where T : Enum => 1 << Convert.ToInt32(layer);
    }
}