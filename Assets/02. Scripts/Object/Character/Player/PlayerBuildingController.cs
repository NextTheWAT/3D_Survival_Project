using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Utils.Extension;
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
        [SerializeField] private LayerMask layerMask;

        [Header("Test Settings")]
        [SerializeField] private GameObject prefab;

        [Header("Debug Information")]
        [SerializeField] private GameObject material;
        [SerializeField] private bool isSimulating;
        [SerializeField] private bool isAxisChanged;

        private CharacterControls _controls;

        private PlayerPerspectiveController _controller;

        private int _origin;
        private CollisionDetector _detector;
        private BuildingSimulationRenderer _renderer;

        private float _degree;

        private int _groundLayerMask;
        private int _notThroughLayerMask;

        private void Awake()
        {
            _controls = new CharacterControls();

            _controller = GetComponent<PlayerPerspectiveController>();

            _groundLayerMask = 1 << LayerMask.NameToLayer(Layer.Ground);
            _notThroughLayerMask = 1 << LayerMask.NameToLayer(Layer.NotThroughable);
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

            if (Physics.Raycast(position, direction, out var hit, length, layerMask))
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

                _detector.Detect();
                _renderer.SetSimulationColor(_detector.IsCollisionDetected == false);

#if UNITY_EDITOR

                Debug.DrawLine(position, hit.point, Color.green);
                Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);

#endif
            }
        }

        private void Build(InputAction.CallbackContext context)
        {
            if (_detector.IsCollisionDetected)
            {
                return;
            }

            _renderer.SetDefaultColor();

            isSimulating = false;
            
            material.layer = _origin;
            var rigidbody = material.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;

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

            material.layer = _origin;

            ObjectPoolingManager.Release(material);

            _detector = null;
            _renderer = null;
            material = null;
        }

        public void Simulate(GameObject instance)
        {
            isSimulating = true;

            material = ObjectPoolingManager.Spawn(instance);
            material.SetActive(true);

            _origin = material.layer;
            material.SetLayerRecursively(Layer.Simulation);

            _detector = material.GetComponent<CollisionDetector>();
            _renderer = material.GetComponent<BuildingSimulationRenderer>();
        }

        public void SetMatertial(GameObject instance)
        {
            material = instance;
        }
    }
}
