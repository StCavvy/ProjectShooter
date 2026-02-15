using System.Threading;
using InputActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float mouseSensitivity;
        [SerializeField] private float _cameraVerticalRotationAngle;
        private PlayerControls _playerControls;
        private Rigidbody _rigidbody;
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private float verticalRotation;


        private void Awake()
        {
            _playerControls = new PlayerControls();
            _rigidbody = GetComponent<Rigidbody>();

            _playerControls.PlayerControllerActions.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _playerControls.PlayerControllerActions.Move.canceled += ctx => _moveInput = Vector2.zero;

            _playerControls.PlayerControllerActions.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
            _playerControls.PlayerControllerActions.Look.canceled += ctx => _lookInput = Vector2.zero;

            _camera.enabled = true;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleRotation();
        }

        private void HandleMovement()
        {
            Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;

            _rigidbody.AddForce(move * _moveSpeed, ForceMode.Force);
        }
        private void HandleRotation()
        {
            float mouseX = _lookInput.x * mouseSensitivity;
            float mouseY = _lookInput.y * mouseSensitivity;

            transform.Rotate(Vector3.up * mouseX);

            verticalRotation -= mouseY;
            verticalRotation = Mathf.Clamp(verticalRotation, -_cameraVerticalRotationAngle, _cameraVerticalRotationAngle);
            _camera.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }


        private void OnEnable()
        {
            _playerControls.PlayerControllerActions.Enable();

        }

        private void OnDisable()
        {
            _playerControls.PlayerControllerActions.Disable();
        }

    }
}

