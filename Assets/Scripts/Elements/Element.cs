using UnityEngine;

namespace Elements
{
    [CreateAssetMenu(menuName = "Game/New Element", fileName = "Unnamed Element")]
    public class Element : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string displayName;

        public Sprite Icon => icon;
        public string DisplayName => displayName;

        private static Element _fire;
        private static Element _water;
        private static Element _frost;
        private static Element _electric;
        
        public static Element Fire => _fire;
        public static Element Water => _water;
        public static Element Frost => _frost;
        public static Element Electric => _electric;

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
        }
    }
}