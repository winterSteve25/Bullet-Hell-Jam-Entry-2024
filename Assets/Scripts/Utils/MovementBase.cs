using UnityEngine;

namespace Utils
{
    public class MovementBase : MonoBehaviour
    {
        public float speed = 4;
        protected Vector2 _normalizeVec;

        protected Rigidbody2D Rigidbody;

        protected virtual void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Rigidbody.velocity = _normalizeVec * (speed * Time.fixedDeltaTime * 100);
        }
    }
}
