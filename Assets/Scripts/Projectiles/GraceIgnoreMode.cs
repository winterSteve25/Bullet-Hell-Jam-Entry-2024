using System;
using UnityEngine;

namespace Projectiles
{
    public enum GraceIgnoreMode
    {
        Player,
        Enemies,
    }

    public static class GraceIgnoreModeExt
    {
        public static int GetLayerMask(this GraceIgnoreMode ignoreMode)
        {
            return ignoreMode switch
            {
                GraceIgnoreMode.Player => LayerMask.GetMask("Default", "Environment", "Objects", "Enemies"),
                GraceIgnoreMode.Enemies => LayerMask.GetMask("Default", "Environment", "Objects", "Player"),
                _ => throw new ArgumentOutOfRangeException(nameof(ignoreMode), ignoreMode, null)
            };
        }
    }
}