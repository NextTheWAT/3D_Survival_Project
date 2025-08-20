using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Management.Pooling
{
    public class ObjectPooling
    {
        private readonly Dictionary<GameObject, Container<GameObject>> _instances;
        private readonly Dictionary<GameObject, Container<GameObject>> _clones;

        private readonly Transform _transform;
        
        public ObjectPooling()
        {
            _instances = new Dictionary<GameObject, Container<GameObject>>();
            _clones = new Dictionary<GameObject, Container<GameObject>>();
        }

        public ObjectPooling(Transform transform) : this()
        {
            _transform = transform;
        }
        
        private GameObject Instantiate(GameObject original, Transform parent)
        {
            parent = parent ?? _transform;
            var instance = UnityEngine.Object.Instantiate(original, parent, true);
            
            // Rename instantiated game object.
            var index = instance.name.IndexOf("(Clone)", StringComparison.Ordinal);
            if (index > 0)
            {
                instance.name = instance.name.Substring(0, index);
            }
            
            instance.SetActive(false);

            return instance;
        }

        public void Create(GameObject original, Transform parent, int capacity)
        {
            if (_instances.ContainsKey(original))
            {
                Debug.LogError($"Pool for object type {original.name} has already been created");
            }

            // Instantiate new object to use.
            var container = new Container<GameObject>(capacity, () => Instantiate(original, parent));
            _instances[original] = container;
        }

        public void Release(GameObject clone)
        {
            // Object deactivate
            clone.SetActive(false);

            if (_clones.ContainsKey(clone))
            {
                // Release and remove deactivated object.
                _clones[clone].Release(clone);
                _clones.Remove(clone);
            }
        }

        public GameObject Spawn(GameObject original, Vector3? position, Quaternion? rotation, Transform parent)
        {
            // Object pool is not exist.
            if (_instances.ContainsKey(original) == false)
            {
                Create(original, parent ?? _transform, 1);
            }
            
            // Get original object.
            var instance = _instances[original];

            // Duplicate original object and set position, rotation and set duplicated object to enable.
            var clone = instance.Component;
            clone.transform.SetPositionAndRotation(position ?? Vector3.zero, rotation ?? Quaternion.identity);

            // Add activated object in object pool.
            _clones.Add(clone, instance);

            return clone;
        }

        public T Spawn<T>(GameObject original, Vector3? position, Quaternion? rotation, Transform parent) where T : MonoBehaviour
        {
            return Spawn(original, position, rotation, parent).GetComponent<T>();
        }
    }
}
