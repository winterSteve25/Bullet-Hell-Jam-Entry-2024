using System.Collections;
using Effects;
using Player;
using Projectiles;
using UnityEngine;
using Utils;

namespace Enemies
{
    public class PlantBommer : Enemy
    {
        [SerializeField]
        private GameObject plantBarrel;

        [SerializeField]
        private float runAwayRangeSqr;

        [SerializeField]
        private float rangeSqr;

        [SerializeField]
        private float timeToExplode;

        [SerializeField]
        private Front front;

        private bool _isExploding;
        private HitPoints _player;

        private void Update()
        {
            var transform1 = transform;
            Vector3 rotation = transform1.eulerAngles;
            rotation.z += 1;
            transform1.eulerAngles = rotation;

            Vector2 playerPos = PlayerMovement.PlayerPos;
            Vector2 currPos = transform1.position;
            Debug.Log((playerPos - currPos).sqrMagnitude);
            if (!_isExploding)
            {
                if (HasLineOfSight(playerPos, currPos, rangeSqr))
                {
                    Vector2 dir = playerPos - currPos;
                    if (dir.sqrMagnitude < runAwayRangeSqr)
                    {
                        Debug.Log("Lmao");
                        StartCoroutine(Boom());
                        return;
                    }
                    Move(dir, false);
                }
                else
                {
                    Vector2 dir = playerPos - currPos;
                    dir = GoLeftOrRight(dir, rangeSqr);
                    Move(dir, false);
                }
            }
        }

        private IEnumerator Boom()
        {
            _isExploding = true;
            _normalizeVec = Vector2.zero;
            yield return new WaitForSeconds(timeToExplode);
            if (front is not null && front.CompareTag("Player"))
                front.GetComponent<HitPoints>().Hp -= 5;
            GameObject barrel = Instantiate(plantBarrel, transform.position, Quaternion.identity);
            barrel.GetComponent<HitPoints>().Hp--;
            Destroy(this.gameObject);
        }
    }
}
