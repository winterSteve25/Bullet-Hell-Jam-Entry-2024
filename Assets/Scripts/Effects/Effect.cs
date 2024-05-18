using UnityEngine;

namespace Effects
{
    [CreateAssetMenu(menuName = "Game/New Effect", fileName = "New Effect")]
    public class Effect : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string displayName;
        [SerializeField] private Color color;

        public Sprite Icon => icon;
        public string DisplayName => displayName;
        public Color Color => color;

        private static Effect _fire;
        private static Effect _water;
        private static Effect _electricity;
        private static Effect _plant;
        private static Effect _earth;
        private static Effect _wind;

        public static Effect Fire => _fire;
        public static Effect Water => _water;
        public static Effect Electricity => _electricity;
        public static Effect Plant => _plant;
        public static Effect Earth => _earth;
        public static Effect Wind => _wind;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            _fire = Resources.Load<Effect>("Effects/Fire");
            _water = Resources.Load<Effect>("Effects/Water");
            _electricity = Resources.Load<Effect>("Effects/Electricity");
            _plant = Resources.Load<Effect>("Effects/Plant");
            _earth = Resources.Load<Effect>("Effects/Earth");
            _wind = Resources.Load<Effect>("Effects/Wind");
        }
    }
}
