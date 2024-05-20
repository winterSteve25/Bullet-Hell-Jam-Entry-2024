using System;
using Effects;
using Effects.Status;
using Procedural;
using UnityEngine;

namespace Player
{
    public class PlayerEffectsClearer : MonoBehaviour
    {
        private EffectObject _effectObject;

        private void Start()
        {
            _effectObject = GetComponent<EffectObject>();
        }

        private void OnEnable()
        {
            RoomTrigger.RoomCompleted += ClearNegativeEffects;
        }

        private void OnDisable()
        {
            RoomTrigger.RoomCompleted -= ClearNegativeEffects;
        }

        private void ClearNegativeEffects()
        {
            for (int i = 0; i < _effectObject.statusEffects.Count; i++)
            {
                StatusEffect statusEffect = _effectObject.statusEffects[i];
                if (statusEffect is Bounded or Steamed)
                {
                    _effectObject.statusEffects.RemoveAt(i);
                    i--;
                    Destroy(statusEffect);
                }
            }
        }
    }
}
