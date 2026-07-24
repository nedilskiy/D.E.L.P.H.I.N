using TMPro;
using UnityEngine;

namespace Delphin.UI
{
    public class DocumentReaderView : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI bodyText;

        public void Show(string title, string body)
        {
            if (titleText != null)
                titleText.text = title;

            if (bodyText != null)
                bodyText.text = body;

            panel.SetActive(true);
        }

        public void Hide()
        {
            panel.SetActive(false);
        }
    }
}
