using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Utils.Input;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Object
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerCharacterController : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Transform container;
        [SerializeField] private Vector3 firstPersonPosition;
        [SerializeField] private Vector3 thirdPersonPosition;

        [Header("Perspective Settings")]
        [SerializeField] private Range range;
        [Range(0f, 1f)]
        [SerializeField] private float sensitivity;

        [Header("Movement Settings")]
        [SerializeField] private float movementForce;
        [SerializeField] private float jumpingForce;
        [Range(0f, 1f)]
        [SerializeField] private float gravityScale;

        [Header("Collision Detection Settings")]
        [SerializeField] private Vector3 center;
        [SerializeField] private Vector3 size;

        [Header("Debug Informations")]
        [SerializeField] private bool isThirdPersonPerspective;
        [SerializeField] private bool isJumping;

        private Rigidbody _rigidbody;
        private Vector3 _direction;
        private float _vertical;

        private int _groundLayerMask;

        private CharacterControls _controls;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            _controls = new CharacterControls();

            _groundLayerMask = 1 << LayerMask.NameToLayer(Layer.Ground);
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnEnable()
        {
            _controls.Enable();
            _controls.Player.Move.performed += Move;
            _controls.Player.Move.canceled += Stop;
            _controls.Player.Jump.performed += Jump;
            _controls.Player.Look.performed += Look;
            _controls.Player.Toggle.performed += Toggle;
        }

        private void FixedUpdate()
        {
            isJumping = Physics.CheckBox(transform.position + center, size, transform.rotation, _groundLayerMask) == false;
            if (isJumping)
            {
                var gravity = -Physics.gravity.y * gravityScale;

                _rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
            }

            var velocity = _direction * movementForce;
            velocity.y = _rigidbody.velocity.y;

            _rigidbody.velocity = velocity;
        }

        private void OnDisable()
        {
            _controls.Player.Move.performed -= Move;
            _controls.Player.Move.canceled -= Stop;
            _controls.Player.Jump.performed -= Jump;
            _controls.Player.Look.performed -= Look;
            _controls.Player.Toggle.performed -= Toggle;
            _controls.Disable();
        }

        private void Destroy()
        {
            _controls = null;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            var position = transform.position;

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(position + center, size);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(position, new Vector3(position.x, position.y, position.z + range.length));
        }

#endif

        private void Move(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector3>();

            _direction = transform.forward * direction.z + transform.right * direction.x;
        }

        private void Stop(InputAction.CallbackContext context)
        {
            _direction = Vector3.zero;
        }

        private void Jump(InputAction.CallbackContext context)
        {
            var mass = _rigidbody.mass;
            var gravity = -Physics.gravity.y;
            var force = Mathf.Sqrt(gravity * jumpingForce) * mass;

            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);
        }

        private void Look(InputAction.CallbackContext context)
        {
            var delta = context.ReadValue<Vector2>();
            var movenent = delta * sensitivity;

            _vertical = Mathf.Clamp(_vertical + movenent.y, range.min, range.max);
            
            container.localEulerAngles = new Vector3(-_vertical, 0f, 0f);
            transform.eulerAngles += new Vector3(0f, movenent.x, 0f);
        }

        private void Toggle(InputAction.CallbackContext context)
        {
            if (isThirdPersonPerspective)
            {
                container.localPosition = firstPersonPosition;

                isThirdPersonPerspective = false;
            }
            else
            {
                container.localPosition = thirdPersonPosition;

                isThirdPersonPerspective = true;
            }
        }

        #region SERIALIZABLE STRUCTURE API

        [Serializable]
        private struct Range
        {
            public float length;
            public float max;
            public float min;
        }

        #endregion
    }
}
