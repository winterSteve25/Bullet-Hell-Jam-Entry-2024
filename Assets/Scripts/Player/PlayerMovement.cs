using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerMovement : MovementBase
    {
        private static Vector2 _playerPos;
        public static Vector2 PlayerPos => _playerPos;

        private static Vector2 _playerVel;
        public static Vector2 PlayerVel => _playerVel;

        private static float _playerSpeed;
        public static float PlayerSpeed => _playerSpeed;

        private HitPoints _hp;
        private float _elapsedTime;

        [SerializeField] private float dashMultiplier = 6f;
        [SerializeField] private float dashCooldown = 1.5f;
        protected override void Start()
        {
            base.Start();
            _hp = GetComponent<HitPoints>();
            _playerSpeed = speed;
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            _normalizeVec = new Vector2(GameInput.GetAxisRaw("Horizontal"), GameInput.GetAxisRaw("Vertical")).normalized;

            if (GameInput.KeyboardKeyDown(KeyCode.Space) && Rigidbody.velocity.sqrMagnitude > 1 && _elapsedTime > dashCooldown)
            {
                _hp.SetInvincible();
                Rigidbody.AddForce(new Vector2(GameInput.GetAxis("Horizontal"),GameInput.GetAxis("Vertical")).normalized
                * dashMultiplier);
                _elapsedTime = 0;
            }

            _playerPos = Rigidbody.position;
            _playerVel = Rigidbody.velocity;
        }
    }
}
