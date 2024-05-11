using Elements;
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

        public void Spawn(
            Vector3 position,
            Positioner.Position offset,
            Positioner.Position positioner,
            ElementStack element,
            int amount = 5,
            float speed = 1f
        )
        {
            for (int i = 0; i < amount; i++)
            {
                Projectile projectile = _projectiles.Get();
                projectile.transform.position = position + offset(position, i);
                projectile.Init(positioner, speed, element, o => _projectiles.Release(o));
            }
        }
    }
}