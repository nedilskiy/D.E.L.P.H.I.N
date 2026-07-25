using System;
using System.Collections;
using UnityEngine;

namespace Delphin.Gameplay
{
    public class CameraFocusController : MonoBehaviour
    {
        [SerializeField] private float moveDuration = 0.5f;

        private Coroutine routine;
        private Vector3 returnPosition;
        private Quaternion returnRotation;

        public void FocusOn(Transform target)
        {
            returnPosition = transform.position;
            returnRotation = transform.rotation;
            StartLerp(target.position, target.rotation, null);
        }

        public void ReturnToPlayer(Action onComplete = null)
        {
            StartLerp(returnPosition, returnRotation, onComplete);
        }

        private void StartLerp(Vector3 targetPosition, Quaternion targetRotation, Action onComplete)
        {
            if (routine != null)
                StopCoroutine(routine);

            routine = StartCoroutine(LerpRoutine(targetPosition, targetRotation, onComplete));
        }

        private IEnumerator LerpRoutine(Vector3 targetPosition, Quaternion targetRotation, Action onComplete)
        {
            var startPosition = transform.position;
            var startRotation = transform.rotation;
            var elapsed = 0f;

            while (elapsed < moveDuration)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / moveDuration);
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                yield return null;
            }

            transform.position = targetPosition;
            transform.rotation = targetRotation;
            routine = null;
            onComplete?.Invoke();
        }
    }
}
