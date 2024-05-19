using Effects;
using Effects.Status;
using Procedural;
using Projectiles;
using UnityEngine;
using Utils;

namespace Enemies
{
    public class EarthGuy : Enemy
    {
        [SerializeField] private float shieldDuration;
        [SerializeField] private float shieldCooldown;
        [SerializeField] private int shieldStrength;
        [SerializeField] private float shootCooldown;
        [SerializeField] private float projectileSpeed;

        private float _lastShot;
        private float _lastShielded;
        private float _lastChangeDir;
        private float _walkDuration;

        protected override void Start()
        {
            base.Start();
            _walkDuration = Random.Range(1, 1.5f);
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _lastShot += dt;
            _lastShielded += dt;
            _lastChangeDir += dt;

            if (_lastShielded > shieldCooldown)
            {
                Shield();
            }

            if (_lastShot > shootCooldown)
            {
                Shoot();
            }

            if (_lastChangeDir > _walkDuration)
            {
                _lastChangeDir = 0;
                Vector2 dir = DirectionExt.Random(null).ToVectorOffset().Rotate(Random.Range(0, 60));
                Move(new Vector2(dir.x, dir.y));
                _walkDuration = Random.Range(1, 1.5f);
            }
        }

        private void Shield()
        {
            _lastShielded = 0;

            if (TryGetComponent(out Shielded shielded))
            {
                shielded.RefreshDuration();
            }

            StatusEffect.Add<Shielded>(EffectObject, s => s.Init(shieldDuration, shieldStrength));
        }

        private void Shoot()
        {
            _lastShot = 0;
            var transform1 = transform;
            Vector3 currPos = transform1.position;

            ProjectileManager.Spawn(
                currPos,
                Positioner.Circle(4, 1, transform1.eulerAngles.z % 360),
                Positioner.Outward(currPos),
                Effect.Earth,
                10,
                IgnoreMode.Enemies,
                amount: 4,
                speed: projectileSpeed,
                onHitAnyObject: obj =>
                {
                    if (!obj.CompareTag("Droplet")) return;
                    Destroy(obj);
                }
            );
        }
    }
}
