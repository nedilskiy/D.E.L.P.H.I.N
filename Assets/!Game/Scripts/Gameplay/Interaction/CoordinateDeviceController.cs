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
            var hasKatushka = katushkaSlot != null && katushkaSlot.IsFilled;
            var hasCassette = cassetteSlot != null && cassetteSlot.IsFilled;
            var success = hasKatushka && hasCassette;

            SetDisplay(success);

            if (!success)
                return;

            cassetteSlot.RecordData(coordinates);
            katushkaSlot.BurnAndEject();
        }

        private void OnIncompatibleCassette()
        {
            if (display != null)
                display.text = "НЕПОДХОДЯЩАЯ КАССЕТА";
        }

        private void SetDisplay(bool showResult)
        {
            if (display == null)
                return;

            display.text = showResult
                ? $"X: {coordinates.x:0.0}\nY: {coordinates.y:0.0}\nZ: {coordinates.z:0.0}"
                : "X: --\nY: --\nZ: --";
        }
    }
}
