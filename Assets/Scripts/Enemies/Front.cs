using UnityEngine;

namespace Enemies
{
    public class Front : MonoBehaviour
    {
        public Collider2D other;

        private void Start()
        {
            other = null;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            this.other = other;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (this.other != other) return;
            this.other = null;
        }
    }
}
