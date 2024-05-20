using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Utils
{
    public class PostProcessingInstance : MonoBehaviour
    {
        private static PostProcessingInstance _instance;

        [SerializeField] private Volume volume;

        private void Awake()
        {
            _instance = this;
        }

        public static void EnableVignette()
        {
            if (!_instance.volume.profile.TryGet(out Vignette vignette))
            {
                return;
            }
            vignette.active = true;
            DOTween.To(() => vignette.intensity.value, f => vignette.intensity.value = f, 0.55f, 0.5f);
        }

        public static void DisableVignette()
        {
            if (!_instance.volume.profile.TryGet(out Vignette vignette))
            {
                return;
            }

            DOTween.To(() => vignette.intensity.value, f => vignette.intensity.value = f, 0, 0.5f)
                .OnComplete(() => vignette.active = true);
        }
    }
}
