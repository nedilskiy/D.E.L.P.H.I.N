using Delphin.Core;
using Delphin.Services;
using TMPro;
using UnityEngine;

namespace Delphin.UI
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform gridParent;
        [SerializeField] private InventorySlotView slotPrefab;
        [SerializeField] private GameObject detailsRoot;
        [SerializeField] private TextMeshProUGUI selectedNameText;
        [SerializeField] private TextMeshProUGUI selectedDescriptionText;

        private IInputService input;
        private IInventoryService inventory;
        private IGameStateService gameState;
        private bool isOpen;

        private void Start()
        {
            input = ServiceLocator.Get<IInputService>();
            inventory = ServiceLocator.Get<IInventoryService>();
            gameState = ServiceLocator.Get<IGameStateService>();

            input.InventoryTogglePerformed += Toggle;
            inventory.ItemsChanged += Refresh;

            SetOpen(false);
        }

        private void OnDestroy()
        {
            if (input != null)
                input.InventoryTogglePerformed -= Toggle;

            if (inventory != null)
                inventory.ItemsChanged -= Refresh;
        }

        private void Toggle()
        {
            if (!isOpen && gameState.CurrentState != GameState.Playing)
                return;

            SetOpen(!isOpen);
        }

        private void SetOpen(bool open)
        {
            isOpen = open;
            panel.SetActive(open);

            Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = open;
            gameState.SetState(open ? GameState.Paused : GameState.Playing);

            if (open)
                Refresh();

            if (detailsRoot != null)
                detailsRoot.SetActive(false);
        }

        private void Refresh()
        {
            for (var i = gridParent.childCount - 1; i >= 0; i--)
                Destroy(gridParent.GetChild(i).gameObject);

            foreach (var item in inventory.Items)
            {
                var slot = Instantiate(slotPrefab, gridParent);
                slot.Bind(item, ShowDetails);
            }
        }

        private void ShowDetails(ItemDefinition item)
        {
            if (selectedNameText != null)
                selectedNameText.text = item.ItemName;

            if (selectedDescriptionText != null)
                selectedDescriptionText.text = item.Description;

            if (detailsRoot != null)
                detailsRoot.SetActive(true);
        }
    }
}
