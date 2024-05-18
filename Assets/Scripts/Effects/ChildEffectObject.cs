using UnityEngine;

namespace Effects
{
    public class ChildEffectObject : MonoBehaviour
    {
        [SerializeField] private EffectObject parent;

        public EffectObject Parent => parent;
    }
}
