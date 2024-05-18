﻿using Effects;
using Player;
using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class FireTank : Enemy
    {
        [SerializeField] private float rangeSqr;
        [SerializeField] private float projectileSpeed;

        protected override void OnEnable()
        {
            base.OnEnable();
            Hp.OnDamaged += OnDamaged;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Hp.OnDamaged -= OnDamaged;
        }

        private void Update()
        {
            var transform1 = transform;
            Vector3 rotation = transform1.eulerAngles;
            rotation.z += 1;
            transform1.eulerAngles = rotation;

            Vector2 playerPos = PlayerMovement.PlayerPos;
            Vector2 currPos = transform1.position;

            if (HasLineOfSight(playerPos, currPos, rangeSqr))
            {
                Vector2 dir = playerPos - currPos;
                Move(dir, false);
            }
            else
            {
                Vector2 dir = playerPos - currPos;
                dir = GoLeftOrRight(dir, rangeSqr);
                Move(dir, false);
            }
        }

        private void OnDamaged()
        {
            var transform1 = transform;
            Vector2 currPos = transform1.position;
            ProjectileManager.Spawn(
                currPos,
                Positioner.Circle(4, 1, transform1.eulerAngles.z % 360),
                Positioner.Outward(currPos),
                Effect.Fire,
                10,
                GraceIgnoreMode.Enemies,
                amount: 4,
                speed: projectileSpeed
            );
        }
    }
}