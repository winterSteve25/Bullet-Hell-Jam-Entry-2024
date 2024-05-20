using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Player
{
    public class PlayerHealthBarVisual : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;

        private HitPoints _hp;
        [SerializeField]
        private ParticleSystem hurtParticles;

        private void OnEnable()
        {
            _hp = GetComponent<HitPoints>();
            _hp.OnDamaged += HpDecrease;
            _hp.OnHealed += HpIncrease;
        }

        private void OnDisable()
        {
            _hp.OnDamaged -= HpDecrease;
            _hp.OnHealed -= HpIncrease;
        }

        private void HpDecrease()
        {
            SoundsManager.PlaySound("hurt");
            hurtParticles.Play();
            HpChanged();
        }

        private void HpIncrease()
        {
            SoundsManager.PlaySound("heal");

            HpChanged();
        }

        private void HpChanged()
        {
            slider.DOValue((float)_hp.Hp / _hp.MaxHp, 0.1f).SetEase(Ease.InQuad);
        }
    }
}
