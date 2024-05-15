using UnityEngine;

namespace Procedural
{
    public class PremadeRoom : MonoBehaviour
    {
        [SerializeField] private Distance fromSpawn;

        public Distance FromSpawn => fromSpawn;
    }
}
