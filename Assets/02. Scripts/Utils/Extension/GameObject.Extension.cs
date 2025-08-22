using UnityEngine;

namespace Utils.Extension
{
    public static class GameObjectExtension
    {
        public static void SetLayerRecursively(this GameObject gameObject, string layerName)
        {
            var layer = LayerMask.NameToLayer(layerName);
            gameObject.layer = layer;

            foreach (Transform child in gameObject.transform)
            {
                SetLayerRecursively(child.gameObject, layerName);
            }
        }
    }
}
