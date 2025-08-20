using UnityEngine;

namespace Object
{
    [RequireComponent(typeof(CollisionDetector))]
    public class BuildingSimulationRenderer : MonoBehaviour
    {
        [Header("Renderer Settings")]
        [Range(0f, 1f)]
        [SerializeField] private float alpha = 0.4f;

        private Renderer _renderer;
        private Material _material;

        private CollisionDetector _detector;

        private Color _green;
        private Color _red;
        private Color _default;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _material = _renderer.material;

            _detector = GetComponent<CollisionDetector>();

            _green = new Color(0f, 1f, 0f, alpha);
            _red = new Color(1f, 0f, 0f, alpha);
            _default = _material.color;
        }

        public void SetSimulationColor(int layerMask)
        {
            var rotation = transform.rotation;
            var color = _detector.IsCollisionDetected(layerMask) ? _red : _green;

            _material.color = color;
        }

        public void SetDefaultColor()
        {
            _material.color = _default;
        }
    }
}
