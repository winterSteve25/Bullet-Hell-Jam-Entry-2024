using System;
using UnityEngine;

namespace Projectiles
{
    public enum IgnoreMode
    {
        Player,
        Enemies,
    }

    public static class GraceIgnoreModeExt
    {
        public static int GetLayerMask(this IgnoreMode ignoreMode)
        {
            return ignoreMode switch
            {
                IgnoreMode.Player => LayerMask.GetMask("Default", "Environment", "Enemies"),
                IgnoreMode.Enemies => LayerMask.GetMask("Default", "Environment", "Player"),
                _ => throw new ArgumentOutOfRangeException(nameof(ignoreMode), ignoreMode, null)
            };
        }
    }
}
