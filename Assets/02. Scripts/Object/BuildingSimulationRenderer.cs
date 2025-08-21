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
        private Material[] _materials;
        private int[] _modes;
        private Color[] _colors;

        private CollisionDetector _detector;

        private Color _green;
        private Color _red;
        private Color _default;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _materials = _renderer.materials;

            _detector = GetComponent<CollisionDetector>();

            var length = _materials.Length;
            _modes = new int[length];
            _colors = new Color[length];

            for (var i = 0; i < length; i++)
            {
                _modes[i] = (int)_materials[i].GetFloat("_Mode");
                _colors[i] = _materials[i].color;
            }

            _green = new Color(0f, 1f, 0f, alpha);
            _red = new Color(1f, 0f, 0f, alpha);
        }

        public void SetSimulationColor(bool isEnabled)
        {
            var color = isEnabled ? _green : _red;

            for (var i = 0; i < _materials.Length; i++)
            {
                _materials[i].color = color;

                SetRenderingMode(_materials[i], RenderingMode.Transparent);
            }
        }

        public void SetDefaultColor()
        {
            for (var i = 0; i < _materials.Length; i++)
            {
                _materials[i].color = _colors[i];

                SetRenderingMode(_materials[i], RenderingMode.Opaque);
            }
        }

        private void SetRenderingMode(Material material, RenderingMode mode)
        {
            switch (mode)
            {
                case RenderingMode.Opaque:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    material.SetInt("_ZWrite", 1);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.DisableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = -1;
                    break;
                case RenderingMode.Transparent:
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = 3000;
                    break;
            }
        }

        #region ENUMERATION API

        private enum RenderingMode
        {
            Opaque, Transparent
        }

        #endregion
    }
}
