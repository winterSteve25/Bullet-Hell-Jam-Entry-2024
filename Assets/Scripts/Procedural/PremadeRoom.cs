using UnityEngine;

namespace Procedural
{
    public class PremadeRoom : MonoBehaviour
    {
        [SerializeField] private Distance fromSpawn;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private Vector2Int origin;

        public Distance FromSpawn => fromSpawn;
        public int Width => width;
        public int Height => height;

        #if UNITY_EDITOR
        private void OnDrawGizmos() {
            Vector3 points = new Vector3[8] {
                // bl to br
                new Vector3(origin.x, origin.y),
                new Vector3(origin.x + width, origin.y),
                // bl to tl
                new Vector3(origin.x, origin.y),
                new Vector3(origin.x, origin.y + height),
                // br to tr
                new Vector3(origin.x + width, origin.y),
                new Vector3(origin.x + width, origin.y + height),
                // tl to tr
                new Vector3(origin.x, origin.y + height),
                new Vector3(origin.x + width, origin.y + height),
            };
            Gizmos.DrawLinesList(points);
        }
        #endif
    }
}
