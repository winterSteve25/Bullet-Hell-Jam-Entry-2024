using System;
using DG.Tweening;
using Effects;
using Effects.Status;
using Projectiles;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        public Camera cam;

        [Header("Config")]
        [SerializeField]
        private float bulletSpeed;

        [SerializeField]
        private float bulletPerSecond;

        [SerializeField]
        private float absorptionCooldown;

        [SerializeField]
        private float cooldownReducOnMiss;

        [SerializeField]
        private AbsorptionBullet absorptionBullet;

        [Header("Skills")]
        [SerializeField]
        private float amountElementApplied;

        [SerializeField]
        private int healAmount;

        [SerializeField]
        private float superNovaRadius;

        [SerializeField]
        private int superNovaDamage;

        [SerializeField]
        private int superNovaSelfDamage;

        [SerializeField]
        private float superNovaForce;

        [SerializeField]
        private float crowdControlRadius;

        [SerializeField]
        private float crowdControlDuration;

        [SerializeField]
        private float pushRadius;

        [SerializeField]
        private float pushForce;

        [SerializeField]
        private float shieldDuration;

        [SerializeField]
        private int shieldStrength;

        [SerializeField]
        private float explosionRadius;

        [SerializeField]
        private int explosionDamage;

        [SerializeField]
        private float explosionForce;

        [SerializeField] private ParticleSystem healParticles;
        [SerializeField] private ParticleSystem absorpParticles;

        [Header("UI")]
        public Slider ammoSlider;
        public Image effectIcon;
        public Slider absBulletCoolDown;

        private float _bulletCooldown;
        private float _lastShot;
        private float _absorptionCountdown;
        private bool _absorptionMode;

        private static HitPoints _hp;
        private EffectObject _effectObject;
        private Sprite _blankElement;

        [Header("Debug")]
        [SerializeField]
        private Effect elementSelected;

        [SerializeField]
        private float elementAmount;
        public float maxElementAmount;

        public static HitPoints Hp => _hp;

        public Effect ElementSelected
        {
            get => elementSelected;
            set
            {
                elementSelected = value;
                if (elementSelected is null)
                {
                    effectIcon.sprite = _blankElement;
                    effectIcon.color = Color.white;
                    return;
                }
                effectIcon.sprite = elementSelected.Icon;
                effectIcon.color = elementSelected.Color;
            }
        }

        public float ElementAmount
        {
            get => elementAmount;
            set
            {
                elementAmount = value;
                ammoSlider.DOValue(elementAmount / maxElementAmount, 0.25f).SetEase(Ease.InOutQuad);
            }
        }

        private void Start()
        {
            _blankElement = Resources.Load<Sprite>("Sprites/square");
            _bulletCooldown = 1 / bulletPerSecond;
            _lastShot = _bulletCooldown;
            _hp = GetComponent<HitPoints>();
            _effectObject = GetComponent<EffectObject>();
            ammoSlider.value = 0;
            effectIcon.sprite = _blankElement;
            _hp.OnDamaged += OnDamage;
        }

        private void OnDisable()
        {
            _hp.OnDamaged -= OnDamage;
        }

        private void OnDamage()
        {
            _hp.SetInvincible();
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _lastShot += dt;

            if (!_absorptionMode)
            {
                _absorptionCountdown -= dt;
            }

            if ((absorptionCooldown - _absorptionCountdown) / absorptionCooldown >= 1)
            {
                absBulletCoolDown.value = 1;
            }
            else
            {
                absBulletCoolDown.value = (absorptionCooldown - _absorptionCountdown) / absorptionCooldown;
            }

            if (_absorptionMode)
            {
                if (GameInput.LeftClickButtonDown())
                {
                    SoundsManager.PlaySound("shoot");

                    Vector2 position = transform.position;
                    Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 dir = mousePos - position;
                    AbsorptionBullet bullet = Instantiate(absorptionBullet);
                    bullet.transform.position = position;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                    bullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
                    bullet.Init(
                        hit =>
                        {
                            if (hit is null)
                            {
                                _absorptionCountdown -= cooldownReducOnMiss;
                                return;
                            }
                            SoundsManager.PlaySound("absorp");

                            ElementSelected = hit.InheritElement;
                            ElementAmount = maxElementAmount;
                        },
                        dir.normalized
                    );
                    absorpParticles.Stop();
                    _absorptionMode = false;
                }
            }
            else
            {
                if (ElementAmount > 0 && _lastShot > _bulletCooldown && GameInput.LeftClickButton())
                {
                    SoundsManager.PlaySound("shoot");

                    Vector2 position = transform.position;
                    Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 dir = mousePos - position;
                    _lastShot = 0;

                    ProjectileManager.Spawn(
                        position,
                        Positioner.Zero(),
                        Positioner.Directional(dir),
                        ElementSelected,
                        ElementAmount * 1.5f + 5,
                        IgnoreMode.Player,
                        amount: 1,
                        speed: bulletSpeed
                    );

                    ElementAmount--;
                    if (ElementAmount <= 0)
                    {
                        ElementAmount = 0;
                        ElementSelected = null;
                    }
                }
            }

            if (!GameInput.RightClickButtonDown())
            {
                return;
            }

            if (ElementSelected is null)
            {
                if (_absorptionCountdown > 0)
                {
                    return;
                }

                _absorptionCountdown = absorptionCooldown;
                _absorptionMode = true;
                SoundsManager.PlaySound("absorp_mode");
                absorpParticles.Play();
                return;
            }

            UseSkill();
        }

        private void UseSkill()
        {
            if (ElementSelected == Effect.Water)
            {
                _hp.Hp += healAmount;
                _effectObject.Apply(Effect.Water, amountElementApplied, true);
                healParticles.Play();
            }
            else if (ElementSelected == Effect.Fire)
            {
                SoundsManager.PlaySound("explode");
                SoundsManager.PlaySound("hurt");

                ParticlesUtils.Explode(transform.position, superNovaRadius);
                _hp.Hp -= superNovaSelfDamage;
                // ReSharper disable once Unity.PreferNonAllocApi
                Collider2D[] targets = Physics2D.OverlapCircleAll(
                    transform.position,
                    superNovaRadius,
                    IgnoreMode.Player.GetLayerMask()
                );
                foreach (var target in targets)
                {
                    if (target is null)
                        continue;

                    EffectObject effectObject;

                    if (target.TryGetComponent(out EffectObject obj))
                    {
                        effectObject = obj;
                    }
                    else if (target.TryGetComponent(out ChildEffectObject childEffectObject))
                    {
                        effectObject = childEffectObject.Parent;
                    }
                    else
                    {
                        continue;
                    }

                    bool wasInvincible = effectObject.Invincible;
                    if (wasInvincible)
                    {
                        effectObject.Invincible = false;
                    }
                    effectObject.Hp -= superNovaDamage;
                    effectObject.Invincible = wasInvincible;
                    effectObject.Apply(Effect.Fire, amountElementApplied, true);

                    CombatUtils.AddForce(
                        effectObject.transform.position - transform.position,
                        superNovaForce,
                        effectObject
                    );
                }
            }
            else if (ElementSelected == Effect.Plant)
            {
                Collider2D[] targets = Physics2D.OverlapCircleAll(
                    transform.position,
                    crowdControlRadius,
                    IgnoreMode.Player.GetLayerMask()
                );
                foreach (var target in targets)
                {
                    if (target is null)
                        continue;
                    if (!target.CompareTag("Enemy"))
                        continue;

                    EffectObject effectObject;

                    if (target.TryGetComponent(out EffectObject obj))
                    {
                        effectObject = obj;
                    }
                    else if (target.TryGetComponent(out ChildEffectObject childEffectObject))
                    {
                        effectObject = childEffectObject.Parent;
                    }
                    else
                    {
                        continue;
                    }

                    effectObject.Apply(Effect.Plant, amountElementApplied, true);
                    StatusEffect.Add<Bounded>(effectObject, b => b.Init(crowdControlDuration));
                }
            }
            else if (ElementSelected == Effect.Wind)
            {
                Collider2D[] targets = Physics2D.OverlapCircleAll(
                    transform.position,
                    pushRadius,
                    IgnoreMode.Player.GetLayerMask()
                );
                foreach (var target in targets)
                {
                    if (target is null)
                        continue;
                    if (!target.CompareTag("Enemy"))
                        continue;

                    EffectObject effectObject;

                    if (target.TryGetComponent(out EffectObject obj))
                    {
                        effectObject = obj;
                    }
                    else if (target.TryGetComponent(out ChildEffectObject childEffectObject))
                    {
                        effectObject = childEffectObject.Parent;
                    }
                    else
                    {
                        continue;
                    }

                    CombatUtils.AddForce(
                        effectObject.transform.position - transform.position,
                        pushForce,
                        effectObject
                    );
                    effectObject.Apply(Effect.Wind, amountElementApplied, true);
                }
            }
            else if (ElementSelected == Effect.Earth)
            {
                StatusEffect.Add<Shielded>(
                    _effectObject,
                    s => s.Init(shieldDuration, shieldStrength)
                );
                _effectObject.Apply(Effect.Water, amountElementApplied, true);
            }
            else if (ElementSelected == Effect.Electricity)
            {
                CombatUtils.Explode(
                    transform.position,
                    explosionRadius,
                    explosionForce,
                    explosionDamage,
                    Effect.Electricity,
                    amountElementApplied,
                    IgnoreMode.Player.GetLayerMask()
                );
            }

            ResetElement();
        }

        private void ResetElement()
        {
            ElementAmount = 0;
            ElementSelected = null;
        }
    }
}
