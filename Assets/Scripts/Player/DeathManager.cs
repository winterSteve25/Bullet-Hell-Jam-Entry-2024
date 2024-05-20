using DG.Tweening;
using EasyTransition;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Player
{
    public class DeathManager : MonoBehaviour
    {
        public static bool IsDead;

        [SerializeField] private HitPoints player;
        [SerializeField] private CanvasGroup canvas;
        [SerializeField] private TransitionSettings transitionSettings;

        private void OnEnable()
        {
            IsDead = false;
            player.OnDeath += Die;
            canvas.interactable = false;
            canvas.alpha = 0;
            canvas.blocksRaycasts = false;
        }

        private void OnDisable()
        {
            player.OnDeath -= Die;
        }

        public void Retry()
        {
            Time.timeScale = 1;
            TransitionManager.Instance().Transition(1, transitionSettings, 0);
        }

        public void MainMenu()
        {
            TransitionManager.Instance().Transition(0, transitionSettings, 0);
        }

        private void Die()
        {
            IsDead = true;

            DOTween.To(() => Time.timeScale, f => Time.timeScale = f, 0, 0.5f)
                .SetEase(Ease.OutQuad);

            canvas.interactable = true;
            canvas.blocksRaycasts = true;
            canvas.DOFade(1, 0.5f);
        }
    }
}
