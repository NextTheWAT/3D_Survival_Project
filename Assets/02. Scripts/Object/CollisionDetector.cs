using UnityEngine;

namespace Object
{
    public class CollisionDetector : MonoBehaviour
    {
        [Header("Collision Detection Settings")]
        [SerializeField] private Vector3 center;
        [SerializeField] private Vector3 size;

        
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(transform.position + center, size);
        }

#endif

        public bool IsCollisionDetected(int layerMask)
        {
            return Physics.CheckBox(transform.position + center, size, transform.rotation, layerMask);
        }

        public Vector3 Center => center;

        public Vector3 Size => size;
    }
}
