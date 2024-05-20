using Effects;
using Projectiles;
using UnityEngine;

namespace Enemies
{
    public class WaterHealer : Enemy
    {
        [SerializeField] private float healRadius;
        [SerializeField] private float healCooldown;
        [SerializeField] private int healAmount;
        [SerializeField] private float waterAppRadius;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private ParticleSystem circle;

        private float _lastHeal;

        protected override void Start()
        {
            base.Start();
            ParticleSystem.ShapeModule particlesShape = particles.shape;
            particlesShape.radius = healRadius;
            ParticleSystem.ShapeModule circleShape = circle.shape;
            circleShape.radius = healRadius * 2.5f;
        }

        private void Update()
        {
            _lastHeal += Time.deltaTime;

            if (_lastHeal > healCooldown)
            {
                Heal();
            }
        }

        private void Heal()
        {
            _lastHeal = 0;
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, healRadius);

            foreach (var col in cols)
            {
                if (col is null) continue;
                if (!col.TryGetComponent(out EffectObject effectObject)) return;
                effectObject.Apply(Effect.Water, 10, true);
                effectObject.Hp += healAmount;
                particles.Play();
                circle.Play();
                Shoot();
            }
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, waterAppRadius);
            foreach (var col in cols)
            {
                if (col is null) continue;
                if (!col.TryGetComponent(out EffectObject effectObject)) return;
                effectObject.Apply(Effect.Water, 10, true);
            }
        }

        private void Shoot()
        {
            var transform1 = transform;
            Vector2 currPos = transform1.position;
            ProjectileManager.Spawn(
                currPos,
                Positioner.Circle(8, 1, transform1.eulerAngles.z % 360),
                Positioner.Outward(currPos),
                Effect.Water,
                10,
                IgnoreMode.Enemies,
                amount: 8,
                speed: projectileSpeed
            );
        }

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(position, healRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, waterAppRadius);
        }
    }
}
