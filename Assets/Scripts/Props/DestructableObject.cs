using UnityEngine;
using Utils;

namespace Props
{
    [RequireComponent(typeof(HitPoints))]
    public class DestructableObject : MonoBehaviour
    {
        public GameObject ExplodePar;
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
            GameObject Par = Instantiate(ExplodePar, transform.position, Quaternion.identity);
            Par.GetComponent<ParticleSystem>().startColor = GetComponent<ElementalBarrel>().InheritElement.Color;
            Destroy(Par, 2f);
            Destroy(gameObject);
        }
    }
}
