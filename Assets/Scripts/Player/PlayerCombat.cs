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
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float bulletPerSecond;
        [SerializeField] private float absorptionCooldown;
        [SerializeField] private float absorptionCooldownWhenFail;
        [SerializeField] private AbsorptionBullet absorptionBullet;

        [Header("Skills")]
        [SerializeField] private float amountElementApplied;
        [SerializeField] private int healAmount;
        [SerializeField] private float superNovaRadius;
        [SerializeField] private int superNovaDamage;
        [SerializeField] private int superNovaSelfDamage;
        [SerializeField] private float superNovaForce;
        [SerializeField] private float crowdControlRadius;
        [SerializeField] private float crowdControlDuration;
        [SerializeField] private float pushRadius;
        [SerializeField] private float pushForce;
        [SerializeField] private float shieldDuration;
        [SerializeField] private int shieldStrength;
        [SerializeField] private float explosionRadius;
        [SerializeField] private int explosionDamage;
        [SerializeField] private float explosionForce;

        [Header("UI")]
        public Slider ammoSlider;
        public Image effectIcon;

        private float _bulletCooldown;
        private float _lastShot;
        private float _lastAbsorb;
        private bool _absorptionMode;
        private bool _waitingForHit;

        private static HitPoints _hp;
        private EffectObject _effectObject;
        private Sprite _blankElement;

        [Header("Debug")]
        [SerializeField] private Effect elementSelected;
        [SerializeField] private float elementAmount;
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
                ammoSlider.DOValue(elementAmount / maxElementAmount, 0.25f)
                    .SetEase(Ease.InOutQuad);
            }
        }

        private void Start()
        {
            _blankElement = Resources.Load<Sprite>("Sprites/square");
            _bulletCooldown = 1 / bulletPerSecond;
            _lastAbsorb = absorptionCooldown;
            _lastShot = _bulletCooldown;
            _hp = GetComponent<HitPoints>();
            _effectObject = GetComponent<EffectObject>();
            ammoSlider.value = 0;
            effectIcon.sprite = _blankElement;
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
                        ElementSelected = hit.InheritElement;
                        ElementAmount = maxElementAmount;
                    }, dir.normalized);
                    _absorptionMode = false;
                    _waitingForHit = true;
                }
            }
            else
            {
                if (ElementAmount > 0 && _lastShot > _bulletCooldown && GameInput.LeftClickButton())
                {
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
            if (ElementSelected == Effect.Water)
            {
                _hp.Hp += healAmount;
                _effectObject.Apply(Effect.Water, amountElementApplied, true);
            }
            else if (ElementSelected == Effect.Fire)
            {
                _hp.Hp -= superNovaSelfDamage;
                // ReSharper disable once Unity.PreferNonAllocApi
                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, superNovaRadius, IgnoreMode.Player.GetLayerMask());
                foreach (var target in targets)
                {
                    if (target is null) continue;

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

                    AddForce(effectObject.transform.position - transform.position, superNovaForce, effectObject);
                }
            }
            else if (ElementSelected == Effect.Plant)
            {
                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, crowdControlRadius, IgnoreMode.Player.GetLayerMask());
                foreach (var target in targets)
                {
                    if (target is null) continue;
                    if (!target.CompareTag("Enemy")) continue;

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
                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, pushRadius, IgnoreMode.Player.GetLayerMask());
                foreach (var target in targets)
                {
                    if (target is null) continue;
                    if (!target.CompareTag("Enemy")) continue;

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

                    AddForce(effectObject.transform.position - transform.position, pushForce, effectObject);
                    effectObject.Apply(Effect.Wind, amountElementApplied, true);
                }
            }
            else if (ElementSelected == Effect.Earth)
            {
                StatusEffect.Add<Shielded>(_effectObject, s => s.Init(shieldDuration, shieldStrength));
                _effectObject.Apply(Effect.Water, amountElementApplied, true);
            }
            else if (ElementSelected == Effect.Electricity)
            {
                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, IgnoreMode.Player.GetLayerMask());
                foreach (var target in targets)
                {
                    if (target is null) continue;
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

                    effectObject.Hp -= explosionDamage;
                    effectObject.Apply(Effect.Fire, amountElementApplied, true);

                    AddForce(effectObject.transform.position - transform.position, explosionForce, effectObject);
                }
            }

            ResetElement();
        }

        private void AddForce(Vector2 dir, float multiplier, EffectObject obj)
        {
            if (!obj.TryGetComponent(out Rigidbody2D rb)) return;

            if (rb.velocity != Vector2.zero)
            {
                rb.AddForce(dir * multiplier, ForceMode2D.Impulse);
            }
            {
                rb.AddForce(dir * (multiplier * 0.1f), ForceMode2D.Impulse);
            }
        }

        private void ResetElement()
        {
            ElementAmount = 0;
            ElementSelected = null;
        }
    }
}
