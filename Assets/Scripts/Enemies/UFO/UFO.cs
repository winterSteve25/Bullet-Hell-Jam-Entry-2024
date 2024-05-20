using Effects;
using Projectiles;
using UnityEngine;

namespace Enemies.UFO
{
    public class UFO : Enemy
    {
        private int _stage;
        private Effect[] _effects;

        protected override void Start()
        {
            base.Start();
            _stage = 0;
            _effects = new[]
            {
                Effect.Electricity, Effect.Fire, Effect.Water,
            };
        }

        private void Update()
        {
            Vector2 currPos = transform.position;

            if (_stage == 0)
            {
                ProjectileManager.Spawn(
                    currPos,
                    Positioner.Circle(100, 1, transform.eulerAngles.z % 360),
                    Positioner.Spiral(),
                    Effect.Electricity,
                    10,
                    IgnoreMode.Enemies,
                    100,
                    20
                );
            }
        }
    }
}
