using UnityEngine;
using Utils.Management.Pooling;

namespace Utils.Management
{
    public class ObjectManager : SingletonGameObject<ObjectManager>
    {
        private ObjectPooling _objectPooling;
        
        protected override void OnAwake()
        {
            _objectPooling = new ObjectPooling(transform);
        }
 
        private void Create_Internal(GameObject origin, int capacity)
        {
            _objectPooling.Create(origin, null, capacity);
        }

        private void Create_Internal(GameObject origin, Transform parent, int capacity)
        {
            _objectPooling.Create(origin, parent, capacity);
        }

        private GameObject Spawn_Internal(GameObject origin, Vector3? position = null, Quaternion? rotation = null, Transform parent = null)
        {
            return _objectPooling.Spawn(origin, position, rotation, parent);
        }

        private T Spawn_Internal<T>(GameObject origin, Vector3? position = null, Quaternion? rotation = null, Transform parent = null) where T : MonoBehaviour
        {
            return _objectPooling.Spawn<T>(origin, position, rotation, parent);
        }

        private void Release_Internal(GameObject clone)
        {
            _objectPooling.Release(clone);
        }

        #region STATIC METHOD API

        public static void Create(GameObject original, int capacity)
        {
            Instance.Create_Internal(original, null, capacity);
        }

        public static void Create(GameObject original, Transform parent, int capacity)
        {
            Instance.Create_Internal(original, parent, capacity);
        }

        public static GameObject Spawn(GameObject original, Vector3? position = null, Quaternion? rotation = null, Transform parent = null)
        {
            return Instance.Spawn_Internal(original, position, rotation, parent);
        }

        public static T Spawn<T>(GameObject original, Vector3? position = null, Quaternion? rotation = null, Transform parent = null) where T : MonoBehaviour
        {
            return Instance.Spawn_Internal<T>(original, position, rotation, parent);
        }

        public static void Release(GameObject clone)
        {
            Instance.Release_Internal(clone);
        }

        #endregion
    }
}
