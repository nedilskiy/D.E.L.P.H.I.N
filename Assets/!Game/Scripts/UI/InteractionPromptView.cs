using Delphin.Gameplay;
using TMPro;
using UnityEngine;

namespace Delphin.UI
{
    public class InteractionPromptView : MonoBehaviour
    {
        [SerializeField] private PlayerInteractor interactor;
        [SerializeField] private TextMeshProUGUI promptText;

        private void OnEnable()
        {
            if (interactor != null)
                interactor.FocusChanged += OnFocusChanged;

            SetVisible(false);
        }

        private void OnDisable()
        {
            if (interactor != null)
                interactor.FocusChanged -= OnFocusChanged;
        }

        private void OnFocusChanged(IInteractable interactable)
        {
            if (interactable == null || string.IsNullOrEmpty(interactable.InteractionPrompt))
            {
                SetVisible(false);
                return;
            }

            promptText.text = interactable.InteractionPrompt;
            SetVisible(true);
        }

        private void SetVisible(bool visible)
        {
            promptText.enabled = visible;
        }
    }
}
