using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Utils.Input;
using Utils.Management;

namespace Object.Character.Player
{
    public class BuildingActionController : MonoBehaviour
    {
        [Header("Building Settings")]
        [SerializeField] private float length;
        [Range(0f, 1f)]
        [SerializeField] private float speed;

        [Header("Test Settings")]
        [SerializeField] private GameObject prefab;

        [Header("Debug Information")]
        [SerializeField] private GameObject material;
        [SerializeField] private bool isSimulating;
        [SerializeField] private bool isAxisChanged;

        private CharacterControls _controls;

        private PlayerPerspectiveController _controller;

        private CollisionDetector _detector;
        private BuildingSimulationRenderer _renderer;

        private float _degree;

        private int _groundLayerMask;
        private int _materialLayerMask;

        private void Awake()
        {
            _controls = new CharacterControls();

            _controller = GetComponent<PlayerPerspectiveController>();

            _groundLayerMask = 1 << LayerMask.NameToLayer(Layer.Ground);
            _materialLayerMask = 1 << LayerMask.NameToLayer(Layer.Material);
        }

        private void OnEnable()
        {
            _controls.Enable();
            _controls.Player.Build.performed += Build;
            _controls.Player.SwitchAxis.performed += SwitchAxis;
            _controls.Player.Rotate.performed += Rotate;
            _controls.Player.Cancel.performed += Cancel;
        }

        private void Update()
        {
            if (isSimulating == false)
            {
                return;
            }

            Simulate();
        }

        private void OnDisable()
        {
            _controls.Player.Build.performed -= Build;
            _controls.Player.SwitchAxis.performed -= SwitchAxis;
            _controls.Player.Rotate.performed -= Rotate;
            _controls.Player.Cancel.performed -= Cancel;
            _controls.Disable();
        }

        private void Destroy()
        {
            _controls = null;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (_controller is null)
            {
                return;
            }

            var start = _controller.FirstPerspectiveCameraRig.position;
            var end = start + _controller.PerspectiveCameraRig.forward * length;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(start, end);
        }

#endif

        private void Simulate()
        {
            var position = _controller.FirstPerspectiveCameraRig.position;
            var direction = _controller.PerspectiveCameraRig.forward;

            if (Physics.Raycast(position, direction, out var hit, length, _groundLayerMask))
            {
                var point = hit.point;
                var height = isAxisChanged ? Mathf.Abs(_detector.Center.y - _detector.Size.x / 2f) : 0f;

                var a = Quaternion.FromToRotation(isAxisChanged ? Vector3.right : Vector3.up, hit.normal);
                var b = isAxisChanged ? Quaternion.Euler(_degree, 0f, 0f) : Quaternion.Euler(0f, _degree, 0f);

                material.transform.rotation = a * b;
                material.transform.position = new Vector3(point.x, point.y, point.z);

                if (isAxisChanged)
                {
                    material.transform.position += hit.normal * height;
                }

                _renderer.SetSimulationColor(_materialLayerMask);

#if UNITY_EDITOR

                Debug.DrawLine(position, hit.point, Color.green);
                Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);

#endif
            }
        }

        private void Build(InputAction.CallbackContext context)
        {
            _renderer.SetDefaultColor();

            isSimulating = false;
            
            _detector = null;
            _renderer = null;
            material = null;
        }

        private void SwitchAxis(InputAction.CallbackContext context)
        {
            isAxisChanged = isAxisChanged == false;
        }

        private void Rotate(InputAction.CallbackContext context)
        {
            var delta = context.ReadValue<float>() * speed;

            _degree = ((_degree + delta) % 360 + 360) % 360;
        }

        private void Cancel(InputAction.CallbackContext context)
        {
            if (isSimulating == false)
            {
                return;
            }

            isSimulating = false;

            ObjectPoolingManager.Release(material);

            _detector = null;
            _renderer = null;
            material = null;
        }

#if UNITY_EDITOR

        [ContextMenu("Test/Simulate")]
        private void TestSimulating()
        {
            isSimulating = true;

            material = ObjectPoolingManager.Spawn(prefab);
            material.SetActive(true);

            _detector = material.GetComponent<CollisionDetector>();
            _renderer = material.GetComponent<BuildingSimulationRenderer>();
        }

#endif

        public void SetMatertial(GameObject instance)
        {
            material = instance;
        }
    }
}
