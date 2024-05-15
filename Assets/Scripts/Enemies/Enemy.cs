using Player;
using Projectiles;
using UnityEngine;
using Utils;

namespace Enemies
{
    [RequireComponent(typeof(HitPoints))]
    public class Enemy : MovementBase
    {
        private HitPoints _hp;

        private void OnEnable()
        {
            _hp = GetComponent<HitPoints>();
            _hp.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            _hp.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }

        protected void Move(Vector2 direction)
        {
            direction.Normalize();
            float hv = CalculateVelocity(Rigidbody.velocity.x, direction.x);
            float vv = CalculateVelocity(Rigidbody.velocity.y, direction.y);
            Rigidbody.velocity = new Vector2(hv * speed, vv * speed);
            Rigidbody.velocity.Normalize();
        }

        protected static bool HasLineOfSight(Vector2 target, Vector2 curr, float range)
        {
            RaycastHit2D ray = Physics2D.Raycast(curr, (target - curr).normalized, range, GraceIgnoreMode.Enemies.GetLayerMask());
            return ray.collider is not null && ray.collider.CompareTag("Player");
        }

        protected static Vector2 PredictPlayerPos()
        {
            Vector2 currPos = PlayerMovement.PlayerPos;
            Vector2 currVel = PlayerMovement.PlayerVel;
            return currPos + currVel * (PlayerMovement.PlayerSpeed * 0.8f);
        }
    }
}
