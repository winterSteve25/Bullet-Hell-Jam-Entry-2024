using System;
using Elements;
using UnityEngine;

namespace Props
{
    [RequireComponent(typeof(ElementObject))]
    public class DestructableObject : MonoBehaviour
    {
        private ElementObject _obj;

        private void OnEnable()
        {
            _obj = GetComponent<ElementObject>();
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