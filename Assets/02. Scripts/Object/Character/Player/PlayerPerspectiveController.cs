using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Input;

namespace Object.Character.Player
{
    public class PlayerPerspectiveController : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private GameObject firstPersonCameraObject;
        [SerializeField] private GameObject thirdPersonCameraObject;
        [Range(1f, 6f)]
        [SerializeField] private float sensitivity;
        [SerializeField] private Angle angle;

        [Header("Perspective Settings")]
        [SerializeField] private PerspectiveMode mode;

        [Header("Debug Information")]
        [SerializeField] private bool isCursorLockedMode;
        [SerializeField] private bool isFirstPersonPerspectiveMode;

        private Camera _firstPersonCamera;
        private Camera _thirdPersonCamera;

        private Transform _firstPersonCameraHorizontalRig;
        private Transform _firstPersonCameraVerticalRig;
        private Transform _thirdPersonCameraHorizontalRig;
        private Transform _thirdPersonCameraVerticalRig;

        private PlayerMovementController _controller;

        private CharacterControls _controls;

        private Vector2 _delta;

        private void Awake()
        {
            // Initialize camera components.
            _firstPersonCamera = firstPersonCameraObject.GetComponent<Camera>();
            _thirdPersonCamera = thirdPersonCameraObject.GetComponent<Camera>();

            // Initialize first and third person camera rig transforms.
            _firstPersonCameraVerticalRig = firstPersonCameraObject.transform.parent;
            _firstPersonCameraHorizontalRig = _firstPersonCameraVerticalRig.parent;
            _thirdPersonCameraVerticalRig = thirdPersonCameraObject.transform.parent;
            _thirdPersonCameraHorizontalRig = _thirdPersonCameraVerticalRig.parent;

            // Activate the camera component from the set perspective type.
            isFirstPersonPerspectiveMode = (mode == PerspectiveMode.FirstPerson);
            firstPersonCameraObject.SetActive(isFirstPersonPerspectiveMode);
            thirdPersonCameraObject.SetActive(isFirstPersonPerspectiveMode == false);

            _controls = new CharacterControls();

            _controller = GetComponent<PlayerMovementController>();

            _delta = Vector2.zero;
        }

        private void OnEnable()
        {
            _controls.Enable();
            _controls.Player.Look.performed += Look;
            _controls.Player.Look.canceled += Stop;
            _controls.Player.SwitchCursorState.performed += SwitchCursorState;
            _controls.Player.SwitchPerspectiveMode.performed += SwitchPerspectiveMode;
        }

        private void Update()
        {
            // Set perspective camera rig transforms by perspective mode.
            var verticalRig = isFirstPersonPerspectiveMode ? _firstPersonCameraVerticalRig : _thirdPersonCameraVerticalRig;
            var horizontalRig = isFirstPersonPerspectiveMode ? _firstPersonCameraHorizontalRig : _thirdPersonCameraHorizontalRig;

            if (mode == PerspectiveMode.ThirdPerson)
            {
                RotateThirdPersonCameraRig();
            }

            if (isCursorLockedMode)
            {
                return;
            }

            var x = verticalRig.localEulerAngles.x;
            var y = horizontalRig.localEulerAngles.y;

            var dx = x + _delta.y * sensitivity;
            var dy = y + _delta.x * sensitivity;

            if (dx > 180f)
            {
                dx -= 360f;
            }

            // Set vertical and horizontal rotation to the camera rig transforms.
            var vertical = Vector3.up * dy;
            var horizontal = Vector3.right * (IsRotationAngleValid(dx) ? dx : x);
            verticalRig.localEulerAngles = horizontal;
            horizontalRig.localEulerAngles = vertical;
        }

        private void OnDisable()
        {
            _controls.Player.Look.performed -= Look;
            _controls.Player.Look.canceled -= Stop;
            _controls.Player.SwitchCursorState.performed -= SwitchCursorState;
            _controls.Player.SwitchPerspectiveMode.performed -= SwitchPerspectiveMode;
            _controls.Disable();
        }

        private void Destroy()
        {
            _controls = null;
        }

        private void Look(InputAction.CallbackContext context)
        {
            var delta = context.ReadValue<Vector2>();
            
            _delta = new Vector2(delta.x, -delta.y);
        }

        private void Stop(InputAction.CallbackContext context)
        {
            _delta = Vector2.zero;
        }

        private void RotateThirdPersonCameraRig()
        {
            var direction = _controller.Direction;
            if (direction.sqrMagnitude > 0f == false)
            {
                return;
            }

            var forward = _thirdPersonCameraVerticalRig.TransformDirection(direction);

            var y = _firstPersonCameraHorizontalRig.localEulerAngles.y;
            var dy = Quaternion.LookRotation(forward, Vector3.up).eulerAngles.y;

            if (dy - y > 180f)
            {
                dy -= 360f;
            }
            else if (y - dy > 180f)
            {
                dy += 360;
            }

            var angle = Vector3.up * dy;
            _firstPersonCameraHorizontalRig.eulerAngles = angle;
        }

        private bool IsRotationAngleValid(float value)
        {
            return angle.min < value && value < angle.max;
        }

        private void SwitchCursorState(InputAction.CallbackContext context)
        {
            isCursorLockedMode = isCursorLockedMode == false;

            Cursor.visible = isCursorLockedMode;
            Cursor.lockState = isCursorLockedMode ? CursorLockMode.None : CursorLockMode.Locked;
        }

        private void SwitchPerspectiveMode(InputAction.CallbackContext context)
        {
            isFirstPersonPerspectiveMode = isFirstPersonPerspectiveMode == false;

            mode = isFirstPersonPerspectiveMode ? PerspectiveMode.FirstPerson : PerspectiveMode.ThirdPerson;

            firstPersonCameraObject.SetActive(isFirstPersonPerspectiveMode);
            thirdPersonCameraObject.SetActive(isFirstPersonPerspectiveMode == false);

            switch (mode)
            {
                case PerspectiveMode.FirstPerson:
                {
                    var x = Vector3.right * _thirdPersonCameraVerticalRig.localEulerAngles.x;
                    var y = Vector3.up * _thirdPersonCameraHorizontalRig.localEulerAngles.y;

                    _firstPersonCameraVerticalRig.localEulerAngles = x;
                    _firstPersonCameraHorizontalRig.localEulerAngles = y;

                    break;
                }
                case PerspectiveMode.ThirdPerson:
                {
                    var x = Vector3.right * _firstPersonCameraVerticalRig.localEulerAngles.x;
                    var y = Vector3.up * _firstPersonCameraHorizontalRig.localEulerAngles.y;

                    _thirdPersonCameraVerticalRig.localEulerAngles = x;
                    _thirdPersonCameraHorizontalRig.localEulerAngles = y;

                    break;
                }
            }
        }

        public Transform PerspectiveCameraRig => isFirstPersonPerspectiveMode ? _firstPersonCameraVerticalRig : _thirdPersonCameraVerticalRig;

        public Transform FirstPerspectiveCameraRig => _firstPersonCameraVerticalRig;

        #region ENUMERATION TYPE API

        public enum PerspectiveMode
        {
            FirstPerson,
            ThirdPerson
        }

        #endregion

        #region SERIALIZABLE STRUCTURE API

        [Serializable]
        private struct Angle
        {
            [Range(-90f, 0f)]
            public float min;
            [Range(0f, 90f)]
            public float max;
        }

        #endregion
    }
}
