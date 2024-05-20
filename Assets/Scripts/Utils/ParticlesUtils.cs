using UnityEngine;

namespace Utils
{
    public class ParticlesUtils
    {
        private static ParticleSystem _explosion;
        private static ParticleSystem _enemyDeath;

        public static void Explode(Vector2 origin, float radius)
        {
            _explosion ??= Resources.Load<ParticleSystem>("Prefabs/Particles/Explosion Particles");
            ParticleSystem ps = Object.Instantiate(_explosion);
            ps.transform.position = origin;
            ParticleSystem.MainModule mainModule = ps.main;
            mainModule.startSize = radius;
            ps.Play();
            Object.Destroy(ps.gameObject, 1);
        }

        public static void EnemyDeath(Vector2 origin, Color color)
        {
            _enemyDeath ??= Resources.Load<ParticleSystem>("Prefabs/Particles/Enemy Death");
            ParticleSystem ps = Object.Instantiate(_enemyDeath);
            ps.transform.position = origin;
            ParticleSystem.MainModule mainModule = ps.main;
            mainModule.startColor = color;
            ps.Play();
            Object.Destroy(ps.gameObject, 1);
        }
    }
}
