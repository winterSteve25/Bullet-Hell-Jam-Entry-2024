using System;
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
                () => Instantiate(prefab),
                projectile => projectile.gameObject.SetActive(true),
                projectile => projectile.gameObject.SetActive(false),
                actionOnDestroy: projectile => Destroy(projectile.gameObject),
                defaultCapacity: 100
            );
        }

        public void Spawn(Vector2 position, Vector2 offset, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Projectile projectile = _projectiles.Get();
                projectile.Init(position + offset * amount, Directional(Vector2.up), 5f, o => _projectiles.Release(o));
            }
        }

        public Func<float, Vector2, Vector2> Directional(Vector2 dir)
        {
            return (_, currPos) => currPos + dir;
        }
    }
}