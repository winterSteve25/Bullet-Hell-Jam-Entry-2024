using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class BossHealthBar : MonoBehaviour
    {
        private static BossHealthBar _instance;

        [SerializeField] private TMP_Text text;
        [SerializeField] private Slider slider;
        [SerializeField] private CanvasGroup group;

        private HitPoints _hp;

        private void Awake()
        {
            _instance = this;
        }

        public static void Init(string n, HitPoints hp)
        {
            _instance.text.text = n;
            _instance._hp = hp;
            hp.OnDamaged += _instance.UpdateSlider;
            hp.OnDeath += _instance.Delete;
            _instance.group.DOFade(1, 0.75f);
        }

        private void UpdateSlider()
        {
            slider.DOValue((float)_hp.Hp / _hp.MaxHp, 0.1f);
        }

        private void Delete()
        {
            group.DOFade(0, 0.75f);
        }
    }
}
