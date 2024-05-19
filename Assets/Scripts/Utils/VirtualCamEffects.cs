using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public class VirtualCamEffects : MonoBehaviour
    {
        private static CinemachineVirtualCamera _cinemachineVirtual;

        private void Awake()
        {
            _cinemachineVirtual = GetComponent<CinemachineVirtualCamera>();
        }

        public static void Shake(float intensity, float time, Ease ease = Ease.OutCubic)
        {
            CinemachineBasicMultiChannelPerlin perlin = _cinemachineVirtual.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = intensity;
            DOTween.To(() => perlin.m_AmplitudeGain, f => perlin.m_AmplitudeGain = f, 0, time)
                .SetEase(ease);
        }
    }
}
