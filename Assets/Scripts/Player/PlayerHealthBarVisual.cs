using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Player
{
    public class PlayerHealthBarVisual : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        private HitPoints _hp;

        private void OnEnable()
        {
            _hp = GetComponent<HitPoints>();
            _hp.OnDamaged += HpChanged;
            _hp.OnHealed += HpChanged;
        }

        private void OnDisable()
        {
            _hp.OnDamaged -= HpChanged;
            _hp.OnHealed -= HpChanged;
        }

        private void HpChanged()
        {
            slider.DOValue((float)_hp.Hp / _hp.MaxHp, 0.1f)
                .SetEase(Ease.InQuad);
        }
    }
}
