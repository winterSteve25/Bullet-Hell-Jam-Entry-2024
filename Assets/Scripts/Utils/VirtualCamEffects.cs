﻿using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public class VirtualCamEffects : MonoBehaviour
    {
        private static CinemachineVirtualCamera _cinemachineVirtual;
        private static CinemachineBasicMultiChannelPerlin _perlin;

        private void Awake()
        {
            _cinemachineVirtual = GetComponent<CinemachineVirtualCamera>();
            _perlin = _cinemachineVirtual.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public static void Shake(float intensity, float time, Ease ease = Ease.OutCubic)
        {
            _perlin.m_AmplitudeGain = intensity;
            DOTween.To(() => _perlin.m_AmplitudeGain, f => _perlin.m_AmplitudeGain = f, 0, time)
                .SetEase(ease)
                .SetUpdate(true);
        }

        public static void ZoomOut(float sizek, float time, Ease ease = Ease.OutCubic)
        {
            DOTween.To(() => _cinemachineVirtual.m_Lens.OrthographicSize, f => _cinemachineVirtual.m_Lens.OrthographicSize = f, sizek, time)
                .SetEase(ease)
                .SetUpdate(true);
        }
    }
}
