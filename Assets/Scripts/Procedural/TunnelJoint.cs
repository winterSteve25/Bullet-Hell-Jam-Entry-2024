using System;
using UnityEngine;

namespace Procedural
{
    [Serializable]
    public struct TunnelJoint
    {
        public Vector3Int position;
        public Direction direction;

        public TunnelJoint(Vector3Int position, Direction direction)
        {
            this.position = position;
            this.direction = direction;
        }
    }
}
