using System.Collections;
using Effects;
using Player;
using Projectiles;
using UnityEngine;
using Utils;

namespace Enemies
{
    public class ElectricLazer : Enemy
    {

        [SerializeField]
        private float shootCooldown;

        [SerializeField]
        private float timeBetweenBullets;
        [SerializeField] private Front front;

        private float _elapsedTime;
        private HitPoints _player;


        private void Update()
        {
            Vector2 currPos = transform.position;
            _elapsedTime += Time.deltaTime;

            Vector2 dir = PredictPlayerPos() - currPos;
            LookAt(dir);


            if (_elapsedTime < shootCooldown)
            {
                return;
            }
            StartCoroutine(Shoot());
        }

        private IEnumerator Shoot()
        {
            _elapsedTime = 0;
            if (front is not null && front.CompareTag("Player"))
            front.GetComponent<HitPoints>().Hp --;
            yield return new WaitForSeconds(timeBetweenBullets);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.collider.CompareTag("Player"))
                return;
            other.collider.GetComponent<HitPoints>().Hp--;
        }
    }
}
