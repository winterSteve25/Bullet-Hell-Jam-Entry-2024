using DG.Tweening;
using Effects;
using Projectiles;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Enemies.UFO
{
    public class UFO : Enemy
    {
        private Enemy[] _enemies;
        private Effect[] _effects;

        [SerializeField] private float shootCooldown;
        [SerializeField] private float petalCooldown;
        [SerializeField] private float spawnCooldown;
        [SerializeField] private float spawnRadius;
        [SerializeField] private float minSpawnCount;
        [SerializeField] private float maxSpawnCount;

        private float _lastShot;
        private float _lastPetal;
        private float _lastSpawned;
        private int _attack;

        protected override void Start()
        {
            base.Start();
            RandomizeAttack();

            _enemies = Resources.LoadAll<Enemy>("Prefabs/Enemies");
            _effects = new[]
            {
                Effect.Electricity, Effect.Fire, Effect.Water, Effect.Earth, Effect.Plant,
            };
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            WinScreen.winScreen.group.DOFade(1, 0.25f);
            WinScreen.winScreen.group.interactable = true;
            WinScreen.winScreen.group.blocksRaycasts = true;
        }

        private void RandomizeAttack() => _attack = Random.Range(0, 3);

        private void Update()
        {
            Vector2 currPos = transform.position;
            float dt = Time.deltaTime;
            _lastShot += dt;
            _lastPetal += dt;
            _lastSpawned += dt;

            switch (_attack)
            {
                case 0 when _lastShot > shootCooldown:
                    _lastShot = 0;
                    RandomizeAttack();
                    Spiral(currPos);
                    break;
                case 1 when _lastSpawned > spawnCooldown:
                    _lastSpawned = 0;
                    RandomizeAttack();
                    Spawn(currPos);
                    break;
                case 2 when _lastPetal > petalCooldown:
                    _lastPetal = 0;
                    Petal(currPos);
                    RandomizeAttack();
                    break;
                default:
                    RandomizeAttack();
                    break;
            }
        }

        private void Spiral(Vector2 currPos)
        {
            ProjectileManager.Spawn(
                currPos,
                Positioner.Circle(100, 8, Mathf.PI),
                Positioner.Outward(currPos, 30),
                _effects,
                10,
                IgnoreMode.Enemies,
                100,
                10
            );

            ProjectileManager.Spawn(
                currPos,
                Positioner.Circle(100, 1),
                Positioner.Outward(currPos, -30),
                _effects,
                10,
                IgnoreMode.Enemies,
                100,
                10
            );
        }

        private void Spawn(Vector2 currPos)
        {
            for (int i = 0; i < Random.Range(minSpawnCount, maxSpawnCount + 1); i++)
            {
                Enemy enemy = Instantiate(_enemies[Random.Range(0, _enemies.Length)]);
                enemy.transform.position = (currPos + Random.insideUnitCircle * (spawnRadius - 1));
            }
        }

        private void Petal(Vector2 currPos)
        {
            ProjectileManager.Spawn(
                currPos,
                Positioner.Zero(),
                Positioner.Polar(Mathf.PI * 12, 200, (_, t, i) =>
                {
                    float f = Mathf.Sin(3 * i);
                    return new Vector2(t * f * Mathf.Cos(i), t * f * Mathf.Sin(i)).Rotate(t * 30);
                }),
                _effects,
                10,
                IgnoreMode.Enemies,
                200,
                10
            );
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
    }
}
