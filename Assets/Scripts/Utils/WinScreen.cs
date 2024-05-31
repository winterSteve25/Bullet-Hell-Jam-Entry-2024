using UnityEngine;

namespace Utils
{
    public class WinScreen : MonoBehaviour
    {
        public static WinScreen winScreen;

        public CanvasGroup group;

        private void Start()
        {
            winScreen = this;
        }
    }
}
