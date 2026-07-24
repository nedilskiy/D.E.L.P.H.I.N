using UnityEngine;

namespace Delphin.Services
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Delphin/Item Definition")]
    public class ItemDefinition : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite icon;
        [SerializeField, TextArea] private string description;
        [SerializeField] private GameObject worldModel;

        public string ItemName => itemName;
        public Sprite Icon => icon;
        public string Description => description;
        public GameObject WorldModel => worldModel;
    }
}
