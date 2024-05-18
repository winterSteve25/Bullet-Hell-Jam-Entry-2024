using Effects;
using Projectiles;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        public Camera cam;

        [Header("Config")]
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float bulletPerSecond;
        [SerializeField] private float absorptionCooldown;
        [SerializeField] private float absorptionCooldownWhenFail;
        [SerializeField] private AbsorptionBullet absorptionBullet;

        private float _bulletCooldown;
        private float _lastShot;
        private float _lastAbsorb;
        private bool _absorptionMode;
        private bool _waitingForHit;

        [SerializeField] private Effect _elementSelected;
        [SerializeField] private float _elementAmount;

        private void Start()
        {
            _bulletCooldown = 1 / bulletPerSecond;
            _lastAbsorb = absorptionCooldown;
            _lastShot = _bulletCooldown;
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _lastShot += dt;
            if (!_absorptionMode && !_waitingForHit)
            {
                _lastAbsorb += dt;
            }

            if (_absorptionMode)
            {
                if (GameInput.LeftClickButtonDown())
                {
                    AbsorptionBullet bullet = Instantiate(absorptionBullet);
                    Vector2 position = transform.position;
                    Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 dir = mousePos - position;
                    bullet.transform.position = position;
                    bullet.Init(hit =>
                    {
                        _waitingForHit = false;
                        if (hit is null)
                        {
                            _lastAbsorb = absorptionCooldown - absorptionCooldownWhenFail;
                            return;
                        }

                        _elementSelected = hit.InheritElement;
                        _elementAmount = 100;
                    }, dir.normalized);
                    _absorptionMode = false;
                    _waitingForHit = true;
                }
            }
            else
            {
                if ( _elementAmount > 0 && _lastShot > _bulletCooldown && GameInput.LeftClickButton())
                {
                    Vector2 position = transform.position;
                    Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 dir = mousePos - position;
                    _lastShot = 0;

                    ProjectileManager.Spawn(
                        position,
                        Positioner.Zero(),
                        Positioner.Directional(dir),
                        _elementSelected,
                        _elementAmount * 1.5f + 5,
                        GraceIgnoreMode.Player,
                        amount: 1,
                        speed: bulletSpeed
                    );

                    _elementAmount--;
                    if (_elementAmount <= 0)
                    {
                        _elementAmount = 0;
                        _elementSelected = null;
                    }
                }
            }

            if (!GameInput.RightClickButtonDown())
            {
                return;
            }

            if (_elementSelected is null)
            {
                if (_lastAbsorb < absorptionCooldown)
                {
                    return;
                }

                _lastAbsorb = 0;
                _absorptionMode = true;
                return;
            }

            UseSkill();
        }

        private void UseSkill()
        {
        }
    }
}
