using Effects;
using Player;
using Procedural;
using Projectiles;
using UnityEngine;
using Utils;

namespace Enemies
{
    [RequireComponent(typeof(HitPoints))]
    public class Enemy : MovementBase
    {
        public float inaccuracy;
        public int belongsToRoom;

        public float Speed
        {
            get => speed;
            set => speed = value;
        }

        protected HitPoints Hp;
        protected EffectObject EffectObject;
        private Droplet _dropletPrefab;

        protected virtual void Awake()
        {
            EffectObject = GetComponent<EffectObject>();
            _dropletPrefab = Resources.Load<Droplet>("Prefabs/Droplet");
        }

        protected virtual void OnEnable()
        {
            Hp = GetComponent<HitPoints>();
            Hp.OnDeath += OnDeath;
        }

        protected virtual void OnDisable()
        {
            Hp.OnDeath -= OnDeath;
        }

        protected virtual void OnDeath()
        {
            Droplet d = Instantiate(_dropletPrefab);
            var position = transform.position;
            d.Init(EffectObject.InheritElement, position);
            RoomTrigger.TriggerEnemyDiedInRoom(belongsToRoom);
            ParticlesUtils.EnemyDeath(position, EffectObject.InheritElement.Color);
            Destroy(gameObject);
        }

        public void Move(Vector2 direction, bool shouldTurn = true, float multiplier = 1)
        {
            if (shouldTurn && direction != Vector2.zero)
            {
                LookAt(direction);
            }

            _normalizeVec = direction.normalized * multiplier;
        }

        protected void LookAt(Vector2 dir)
        {
            transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        }

        protected Vector2 GoLeftOrRight(Vector2 direction, float range)
        {
            Vector2 newDir = direction.Rotate(45);
            RaycastHit2D ray = Physics2D.Raycast(transform.position, newDir, range, IgnoreMode.Enemies.GetLayerMask());
            if (ray.collider is null)
            {
                return newDir;
            }

            newDir = direction.Rotate(-90);
            ray = Physics2D.Raycast(transform.position, newDir, range, IgnoreMode.Enemies.GetLayerMask());
            if (ray.collider is null)
            {
                return newDir;
            }

            return direction;
        }

        protected static bool HasLineOfSight(Vector2 target, Vector2 curr, float range)
        {
            RaycastHit2D ray = Physics2D.Raycast(curr, (target - curr).normalized, range, IgnoreMode.Enemies.GetLayerMask());
            return ray.collider is not null && ray.collider.CompareTag("Player");
        }

        protected static Vector2 PredictPlayerPos()
        {
            Vector2 currPos = PlayerMovement.PlayerPos;
            Vector2 currVel = PlayerMovement.PlayerVel;
            return currPos + currVel * (PlayerMovement.PlayerSpeed * Time.deltaTime * 5);
        }
    }
}
