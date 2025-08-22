using UnityEngine;
using Utils;
using Utils.Extension;

namespace Object
{
    public class CollisionDetector : MonoBehaviour
    {
        [Header("Collision Detection Settings")]
        [SerializeField] private Vector3 center;
        [SerializeField] private Vector3 size;
        [SerializeField] private LayerMask layerMask;

        private int _index;

        private bool _isCollisionDetected;

        private void Awake()
        {
            _isCollisionDetected = false;
        }

        public void Detect()
        {
            _isCollisionDetected = Physics.CheckBox(transform.TransformPoint(center), size / 2f, transform.rotation, layerMask);
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            var matrix = Matrix4x4.TRS(transform.TransformPoint(center), transform.rotation, size);

            Gizmos.color = Color.red;
            Gizmos.matrix = matrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }

#endif

        public Vector3 Center => center;

        public Vector3 Size => size;

        public bool IsCollisionDetected => _isCollisionDetected;
    }
}
