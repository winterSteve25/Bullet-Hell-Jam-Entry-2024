using DG.Tweening;
using Player;
using Projectiles;
using UnityEngine;
using Utils;

namespace Enemies
{
    [RequireComponent(typeof(HitPoints))]
    public class Enemy : MovementBase
    {
        protected HitPoints Hp;

        public float inaccuracy;

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        protected virtual void OnEnable()
        {
            Hp = GetComponent<HitPoints>();
            Hp.OnDeath += OnDeath;
        }

        protected virtual void OnDisable()
        {
            Hp.OnDeath -= OnDeath;
        }

        protected virtual void OnDeath()
        {
            Destroy(gameObject);
        }

        public void Move(Vector2 direction, bool shouldTurn = true, float multiplier = 1)
        {
            if (shouldTurn && direction != Vector2.zero)
            {
                LookAt(direction);
            }

            direction.Normalize();
            float hv = CalculateVelocity(Rigidbody.velocity.x, direction.x);
            float vv = CalculateVelocity(Rigidbody.velocity.y, direction.y);
            Rigidbody.velocity = new Vector2(hv * speed * multiplier, vv * speed * multiplier);
            Rigidbody.velocity.Normalize();
        }

        protected void LookAt(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (Mathf.Abs(transform.rotation.eulerAngles.z - angle) < 45)
            {
                return;
            }
            transform.DORotateQuaternion(Quaternion.Euler(0, 0, angle), 0.2f);
        }

        protected Vector2 GoLeftOrRight(Vector2 direction, float range)
        {
            Vector2 newDir = direction.Rotate(45);
            RaycastHit2D ray = Physics2D.Raycast(transform.position, newDir, range, GraceIgnoreMode.Enemies.GetLayerMask());
            if (ray.collider is null)
            {
                return newDir;
            }

            newDir = direction.Rotate(-90);
            ray = Physics2D.Raycast(transform.position, newDir, range, GraceIgnoreMode.Enemies.GetLayerMask());
            if (ray.collider is null)
            {
                return newDir;
            }

            return direction;
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
            return currPos + currVel * (PlayerMovement.PlayerSpeed * Time.deltaTime * 100);
        }
    }
}
