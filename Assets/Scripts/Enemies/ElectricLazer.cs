using System.Collections;
using System.Linq;
using DG.Tweening;
using Effects;
using Player;
using Projectiles;
using UnityEngine;
using Utils;

namespace Enemies
{
    public class ElectricLazer : Enemy
    {
        [SerializeField] private float shootCooldown;
        [SerializeField] private float chargeTime;
        [SerializeField] private float turnSpeed;
        [SerializeField] private int lazerDamage;
        [SerializeField] private LineRenderer lazer;
        [SerializeField] private LineRenderer telegraph;
        [SerializeField] private Transform sprite;
        [SerializeField] private ParticleSystem chargeParticles;

        private float _elapsedTime;
        private bool _charging;
        private bool _readyToFire;
        private HitPoints _player;

        protected override void Start()
        {
            base.Start();
            lazer.widthMultiplier = 0;
            telegraph.widthMultiplier = 0;
        }

        private void Update()
        {
            var transform1 = transform;
            var position = transform1.position;
            if (!_charging) _elapsedTime += Time.deltaTime;

            if (!_readyToFire)
            {
                Vector2 dir = PlayerMovement.PlayerPos - (Vector2)position;
                transform.DORotate(new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg), turnSpeed)
                    .SetEase(Ease.InOutSine);
            }

            if (_elapsedTime < shootCooldown)
            {
                return;
            }

            StartCoroutine(Shoot());
        }

        private IEnumerator Shoot()
        {
            _elapsedTime = 0;
            _charging = true;
            Transform transform1 = transform;

            chargeParticles.Play();

            ParticleSystem.MainModule chargeParticlesMain = chargeParticles.main;
            DOTween.To(() => chargeParticlesMain.startSpeed.constant, f =>
                {
                    ParticleSystem.MinMaxCurve s = chargeParticlesMain.startSpeed;
                    s.constant = f;
                    chargeParticlesMain.startSpeed = s;
                }, 9, chargeTime)
                .SetEase(Ease.InCubic);

            sprite.DOScaleY(.6f, chargeTime)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    Vector2 position = transform1.position;
                    Vector2 right = transform1.right;

                    RaycastHit2D col = Physics2D.Raycast(position, right, 1000, IgnoreMode.Enemies.GetLayerMask());
                    lazer.SetPosition(0, position + right);

                    if (col.collider is not null)
                    {
                        lazer.SetPosition(1, col.point * 1.05f);
                    }
                    else
                    {
                        lazer.SetPosition(1, position + right * 1000);
                    }

                    if (col.collider is not null && col.collider.TryGetComponent(out EffectObject effectObject))
                    {
                        effectObject.Hp -= lazerDamage;
                        if (!effectObject.Invincible)
                        {
                            effectObject.Apply(Effect.Electricity, 10, true);
                        }
                        if (col.collider.CompareTag("Player"))
                        {
                            VirtualCamEffects.Shake(effectObject.Invincible ? 4 : 10, 1.5f);
                        }
                    }

                    DOTween.To(() => lazer.widthMultiplier, f => lazer.widthMultiplier = f, 1, 0.1f)
                        .SetEase(Ease.InCubic)
                        .OnComplete(() => DOTween.To(() => lazer.widthMultiplier, f => lazer.widthMultiplier = f, 0, 0.25f));

                    telegraph.widthMultiplier = 0;
                    _readyToFire = false;

                    sprite.DOScaleY(1, 0.25f)
                        .SetEase(Ease.OutCubic);

                    _charging = false;
                    chargeParticles.Stop();
                    ParticleSystem.MinMaxCurve c = chargeParticlesMain.startSpeed;
                    c.constant = 2;
                    chargeParticlesMain.startSpeed = c;
                });

            yield return new WaitForSeconds(chargeTime * 0.96f);

            _readyToFire = true;
            DOTween.To(() => telegraph.widthMultiplier, f => telegraph.widthMultiplier = f, 0.1f, chargeTime * 0.01f)
                .SetEase(Ease.InCubic);

            Vector2 position = transform1.position;
            Vector2 right = transform1.right;
            telegraph.SetPosition(0, position + right);
            telegraph.SetPosition(1, position + right * 100);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.right * 10);
        }
    }
}
