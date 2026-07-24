using Delphin.Core;
using Delphin.Services;
using UnityEngine;

namespace Delphin.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 3.5f;
        [SerializeField] private float sprintSpeed = 6f;
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float gravity = -19.6f;

        [Header("Look")]
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private float mouseSensitivity = 0.12f;
        [SerializeField] private float minPitch = -85f;
        [SerializeField] private float maxPitch = 85f;

        private CharacterController controller;
        private IInputService input;
        private Vector3 velocity;
        private float pitch;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            input = ServiceLocator.Get<IInputService>();
            input.JumpPerformed += OnJumpPerformed;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnDestroy()
        {
            if (input != null)
                input.JumpPerformed -= OnJumpPerformed;
        }

        private void Update()
        {
            if (Cursor.lockState != CursorLockMode.Locked)
                return;

            Look();
            Move();
        }

        private void Look()
        {
            var look = input.LookInput;
            var yaw = look.x * mouseSensitivity;
            pitch = Mathf.Clamp(pitch - look.y * mouseSensitivity, minPitch, maxPitch);

            transform.Rotate(Vector3.up, yaw);
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private void Move()
        {
            var move = input.MoveInput;
            var speed = input.SprintHeld ? sprintSpeed : walkSpeed;
            var direction = (transform.right * move.x + transform.forward * move.y) * speed;

            if (controller.isGrounded && velocity.y < 0f)
                velocity.y = -2f;

            velocity.x = direction.x;
            velocity.z = direction.z;
            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }

        private void OnJumpPerformed()
        {
            if (controller.isGrounded)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
