using TMPro;
using UnityEngine;

namespace Delphin.Gameplay
{
    public class CoordinateDeviceController : MonoBehaviour
    {
        [SerializeField] private ItemSlotInteractable katushkaSlot;
        [SerializeField] private CassetteSlotInteractable cassetteSlot;
        [SerializeField] private LeverInteractable lever;
        [SerializeField] private TMP_Text display;
        [SerializeField] private Vector3 coordinates;

        private void Start()
        {
            if (lever != null)
                lever.Pulled += OnLeverPulled;

            if (cassetteSlot != null)
                cassetteSlot.IncompatibleCassetteInserted += OnIncompatibleCassette;

            SetDisplay(false);
        }

        private void OnDestroy()
        {
            if (lever != null)
                lever.Pulled -= OnLeverPulled;

            if (cassetteSlot != null)
                cassetteSlot.IncompatibleCassetteInserted -= OnIncompatibleCassette;
        }

        private void OnLeverPulled()
        {
            var hasCassette = cassetteSlot != null && cassetteSlot.IsFilled;
            if (!hasCassette)
            {
                SetMessage("ОТСУТСТВУЕТ НАВИГАЦИОННАЯ КАССЕТА");
                return;
            }

            var hasKatushka = katushkaSlot != null && katushkaSlot.IsFilled;
            SetDisplay(hasKatushka);

            if (!hasKatushka)
                return;

            cassetteSlot.RecordData(coordinates);
            katushkaSlot.BurnAndEject();
        }

        private void OnIncompatibleCassette()
        {
            SetMessage("НЕПОДХОДЯЩАЯ КАССЕТА");
        }

        private void SetDisplay(bool showResult)
        {
            if (display == null)
                return;

            display.text = showResult
                ? $"X: {coordinates.x:0.0}\nY: {coordinates.y:0.0}\nZ: {coordinates.z:0.0}"
                : "X: --\nY: --\nZ: --";
        }

        private void SetMessage(string message)
        {
            if (display != null)
                display.text = message;
        }
    }
}
