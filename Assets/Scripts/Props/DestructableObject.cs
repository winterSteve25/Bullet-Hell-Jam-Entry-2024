using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Props
{
    [RequireComponent(typeof(HitPoints))]
    public class DestructableObject : MonoBehaviour
    {
        private HitPoints _obj;

        private void OnEnable()
        {
            _obj = GetComponent<HitPoints>();
            _obj.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            _obj.OnDeath -= OnDeath;
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}
