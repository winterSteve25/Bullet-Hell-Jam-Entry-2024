using UnityEngine;

namespace Utils
{
    public class ParticlesUtils
    {
        private static ParticleSystem _explosion;

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
    }
}
