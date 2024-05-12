using System;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private static Vector2 _playerPos;
        public static Vector2 PlayerPos => _playerPos;
        
        private Rigidbody2D _rigidbody;
        private HitPoints _hp;
        private float _elapsedTime;

        [SerializeField] private float horizontalDamping = 0.9f;
        [SerializeField] private float horizontalDampingWhenStopping = 0.9f;
        [SerializeField] private float horizontalDampingWhenTurning = 0.9f;
        [SerializeField] private float speed = 0.935f;
        [SerializeField] private float dashMultiplier = 6f;
        [SerializeField] private float dashCooldown = 1.5f;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _hp = GetComponent<HitPoints>();
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            
            float hv = CalculateVelocity(_rigidbody.velocity.x, "Horizontal");
            float vv = CalculateVelocity(_rigidbody.velocity.y, "Vertical");
            _rigidbody.velocity = new Vector2(hv * speed, vv * speed);
            _rigidbody.velocity.Normalize();

            if (GameInput.KeyboardKeyDown(KeyCode.Space) && _rigidbody.velocity.sqrMagnitude > 1 && _elapsedTime > dashCooldown)
            {
                _rigidbody.velocity *= dashMultiplier;
                _hp.SetInvincible();
                _elapsedTime = 0;
            }
            
            _playerPos = _rigidbody.position;
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