using UnityEngine;

namespace Delphin.UI
{
    public class MenuCameraSway : MonoBehaviour
    {
        [SerializeField] private float amplitude = 0.05f;
        [SerializeField] private float speed = 0.5f;

        private Vector3 basePosition;
        private float t;

        private void Start()
        {
            basePosition = transform.localPosition;
        }

        private void Update()
        {
            t += Time.deltaTime * speed;

            var sin = Mathf.Sin(t);
            var offset = new Vector3(sin, sin, 0f * Mathf.Cos(t)) * amplitude;

            transform.localPosition = basePosition + offset;
        }
    }
}
