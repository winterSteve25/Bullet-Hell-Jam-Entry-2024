using UnityEngine;

namespace Effects
{
    [CreateAssetMenu(menuName = "Game/New Effect", fileName = "New Effect")]
    public class Effect : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string displayName;

        public Sprite Icon => icon;
        public string DisplayName => displayName;

        private static Effect _fire;
        private static Effect _water;
        private static Effect _frost;
        private static Effect _electricity;
        private static Effect _oil;
        private static Effect _steam;

        public static Effect Fire => _fire;
        public static Effect Water => _water;
        public static Effect Frost => _frost;
        public static Effect Electricity => _electricity;
        public static Effect Oil => _oil;
        public static Effect Steam => _steam;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _fire = Resources.Load<Effect>("Effects/Fire");
            _water = Resources.Load<Effect>("Effects/Water");
            _frost = Resources.Load<Effect>("Effects/Frost");
            _electricity = Resources.Load<Effect>("Effects/Electricity");
            _oil = Resources.Load<Effect>("Effects/Oil");
            _steam = Resources.Load<Effect>("Effects/Steam");
        }
    }
}