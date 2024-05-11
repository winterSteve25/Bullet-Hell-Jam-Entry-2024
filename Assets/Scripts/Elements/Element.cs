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
    }
}