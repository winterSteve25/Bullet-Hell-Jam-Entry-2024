using System;
using Effects;
using UnityEngine;
using UnityEngine.Pool;

namespace Projectiles
{
    public class ProjectileManager : MonoBehaviour
    {
        private static ProjectileManager _instance;
        public static ProjectileManager Instance => _instance;

        [SerializeField] private Projectile prefab;
        private ObjectPool<Projectile> _projectiles;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _projectiles = new ObjectPool<Projectile>(
                () => Instantiate(prefab, transform),
                projectile => projectile.gameObject.SetActive(true),
                projectile => projectile.gameObject.SetActive(false),
                actionOnDestroy: projectile => Destroy(projectile.gameObject),
                defaultCapacity: 100
            );
        }

        public static void Spawn(
            Vector2 position,
            Positioner.Position offset,
            Positioner.Position positioner,
            Effect effect,
            float effAmount,
            IgnoreMode ignoreMode,
            int amount = 5,
            float speed = 1f,
            Action<EffectObject> onHit = null
        )
        {
            for (int i = 0; i < amount; i++)
            {
                Projectile projectile = Instance._projectiles.Get();
                projectile.transform.position = position + offset(position, i);
                projectile.Init(positioner, speed, effect, effAmount, o => Instance._projectiles.Release(o), ignoreMode.GetLayerMask(), onHit: onHit);
            }
        }
    }
}
