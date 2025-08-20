using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Utils.Input;

namespace Object.Character.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float movementForce;
        [SerializeField] private float jumpingForce;
        [Range(0f, 1f)]
        [SerializeField] private float gravityScale;

        [Header("Collision Detection Settings")]
        [SerializeField] private Vector3 center;
        [SerializeField] private Vector3 size;

        [Header("Debug Informations")]
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isJumping;

        private Rigidbody _rigidbody;
        private Vector3 _direction;
        private float _vertical;

        private int _groundLayerMask;

        private CharacterControls _controls;

        private PlayerPerspectiveController _controller;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _controls = new CharacterControls();

            _controller = GetComponent<PlayerPerspectiveController>();

            _groundLayerMask = 1 << LayerMask.NameToLayer(Layer.Ground);
        }

        private void OnEnable()
        {
            _controls.Enable();
            _controls.Player.Move.performed += Move;
            _controls.Player.Move.canceled += Stop;
            _controls.Player.Jump.performed += Jump;
        }

        private void FixedUpdate()
        {
            isMoving = _direction.sqrMagnitude > 0f;
            isJumping = Physics.CheckBox(transform.position + center, size, transform.rotation, _groundLayerMask) == false;
            
            if (isJumping)
            {
                var gravity = -Physics.gravity.y * gravityScale;

                _rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
            }

            var direction = _controller.PerspectiveCameraRig.TransformDirection(_direction);
            var velocity = direction * movementForce;
            velocity.y = _rigidbody.velocity.y;

            _rigidbody.velocity = velocity;
        }

        private void OnDisable()
        {
            _controls.Player.Move.performed -= Move;
            _controls.Player.Move.canceled -= Stop;
            _controls.Player.Jump.performed -= Jump;
            _controls.Disable();
        }

        private void Destroy()
        {
            _controls = null;
        }

        private void OnDrawGizmos()
        {
            var position = transform.position;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(position + center, size);
        }

        private void Move(InputAction.CallbackContext context)
        {
            _direction = context.ReadValue<Vector3>();
        }

        private void Stop(InputAction.CallbackContext context)
        {
            var velocity = _rigidbody.velocity;

            _rigidbody.velocity = new Vector3(0f, velocity.y, 0f);
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (isJumping)
            {
                return;
            }

            var mass = _rigidbody.mass;
            var gravity = -Physics.gravity.y;
            var force = Mathf.Sqrt(gravity * jumpingForce) * mass;

            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }

        public Vector3 Direction => _direction;
    }
}
