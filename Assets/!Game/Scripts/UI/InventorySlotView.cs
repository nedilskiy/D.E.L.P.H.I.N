using System;
using Delphin.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Delphin.UI
{
    [RequireComponent(typeof(Button))]
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private Image icon;

        private ItemDefinition item;
        private Action<ItemDefinition> onClicked;

        public void Bind(ItemDefinition boundItem, Action<ItemDefinition> onSlotClicked)
        {
            item = boundItem;
            onClicked = onSlotClicked;
            icon.sprite = boundItem.Icon;
        }

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(HandleClick);
        }

        private void HandleClick()
        {
            onClicked?.Invoke(item);
        }
    }
}
