using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class StatusEffectsUI : MonoBehaviour
    {
        private static StatusEffectsUI _instance;

        [SerializeField] private Slider prefab;
        [SerializeField] private RectTransform layout;

        private void Awake()
        {
            _instance = this;
        }

        public static (Action<float>, Action) Add(Sprite icon, Color color)
        {
            Slider slider = Instantiate(_instance.prefab, _instance.layout);
            // Image image = slider.transform.GetChild(2).GetComponent<Image>();
            // image.sprite = icon;
            // image.color = color;
            return (value =>
            {
                slider.DOValue(value, 0.1f);
            }, () =>
            {
                Destroy(slider.gameObject);
            });
        }
    }
}
