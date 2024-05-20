using DG.Tweening;
using UnityEngine;
using Utils;

namespace Player
{
    public class DeathManager : MonoBehaviour
    {
        [SerializeField] private HitPoints player;
        [SerializeField] private CanvasGroup canvas;

        private void OnEnable()
        {
            player.OnDeath += Die;
            canvas.interactable = false;
            canvas.alpha = 0;
            canvas.blocksRaycasts = false;
        }

        private void OnDisable()
        {
            player.OnDeath -= Die;
        }

        private void Die()
        {
            DOTween.To(() => Time.timeScale, f => Time.timeScale = f, 0, 0.5f)
                .SetEase(Ease.OutQuad);

            canvas.interactable = true;
            canvas.blocksRaycasts = true;
            canvas.DOFade(1, 0.5f);
        }
    }
}
