using System;
using UnityEngine;

namespace Utils
{
    public class MovementBase : MonoBehaviour
    {
        [SerializeField] protected float horizontalDamping = 0.9f;
        [SerializeField] protected float horizontalDampingWhenStopping = 0.9f;
        [SerializeField] protected float horizontalDampingWhenTurning = 0.9f;
        [SerializeField] protected float speed = 0.935f;

        protected Rigidbody2D Rigidbody;

        protected virtual void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        protected float CalculateVelocity(float initial, float input)
        {
            float fHorizontalVelocity = initial;
            fHorizontalVelocity += input;

            if (Mathf.Abs(input) < 0.01f)
                fHorizontalVelocity *= Mathf.Pow(1f - horizontalDampingWhenStopping, Time.deltaTime * 10f);
            else if (Math.Abs(Mathf.Sign(input) - Mathf.Sign(fHorizontalVelocity)) > 0.01)
                fHorizontalVelocity *= Mathf.Pow(1f - horizontalDampingWhenTurning, Time.deltaTime * 10f);
            else
                fHorizontalVelocity *= Mathf.Pow(1f - horizontalDamping, Time.deltaTime * 10f);

            return fHorizontalVelocity;
        }
    }
}
