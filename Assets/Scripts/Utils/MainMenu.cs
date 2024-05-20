using EasyTransition;
using UnityEngine;

namespace Utils
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TransitionSettings settings;

        public void NewGame()
        {
            TransitionManager.Instance().Transition(1, settings, 0);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
