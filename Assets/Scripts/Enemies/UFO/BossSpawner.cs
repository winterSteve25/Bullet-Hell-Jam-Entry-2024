using Procedural;
using UnityEngine;
using Utils;

namespace Enemies.UFO
{
    public class BossSpawner : MonoBehaviour
    {
        [SerializeField] private PremadeRoom r;

        private UFO boss;

        private void Start()
        {
            boss = GetComponentInChildren<UFO>();
        }

        private void OnEnable()
        {
            RoomTrigger.EnteredRoom += SpawnBoss;
        }

        private void OnDisable()
        {
            RoomTrigger.EnteredRoom -= SpawnBoss;
        }

        private void SpawnBoss(int room)
        {
            if (room != r.roomIdx) return;
            HitPoints ufo = transform.GetChild(0).GetComponent<HitPoints>();
            ufo.gameObject.SetActive(true);
            BossHealthBar.Init("UFO", ufo);
            VirtualCamEffects.ZoomOut(12, 0.5f);
        }
    }
}
