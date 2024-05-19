using System;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerCamEffects : MonoBehaviour
    {
        private HitPoints _hp;

        private void OnEnable()
        {
            _hp = GetComponent<HitPoints>();
            _hp.OnDamaged += Shake;
        }

        private void OnDisable()
        {
            _hp.OnDamaged -= Shake;
        }

        private void Shake()
        {
            VirtualCamEffects.Shake(7, 1);
        }
    }
}
