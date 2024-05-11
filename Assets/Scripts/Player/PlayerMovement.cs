using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        [SerializeField] private float horizontalDamping = 0.5f;
        [SerializeField] private float horizontalDampingWhenStopping = 0.5f;
        [SerializeField] private float horizontalDampingWhenTurning = 0.5f;
        [SerializeField] private float speed = 0.5f;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            float hv = CalculateVelocity(_rigidbody.velocity.x, "Horizontal");
            float vv = CalculateVelocity(_rigidbody.velocity.y, "Vertical");
            _rigidbody.velocity = new Vector2(hv * speed, vv * speed);
            _rigidbody.velocity.Normalize();
        }

        private float CalculateVelocity(float initial, string axis)
        {
            float fHorizontalVelocity = initial;
            fHorizontalVelocity += Input.GetAxisRaw(axis);

            if (Mathf.Abs(Input.GetAxisRaw(axis)) < 0.01f)
                fHorizontalVelocity *= Mathf.Pow(1f - horizontalDampingWhenStopping, Time.deltaTime * 10f);
            else if (Math.Abs(Mathf.Sign(Input.GetAxisRaw(axis)) - Mathf.Sign(fHorizontalVelocity)) > 0.01)
                fHorizontalVelocity *= Mathf.Pow(1f - horizontalDampingWhenTurning, Time.deltaTime * 10f);
            else
                fHorizontalVelocity *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);

            return fHorizontalVelocity;
        }
    }
}