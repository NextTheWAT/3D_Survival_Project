using System.Collections.Generic;

namespace Utils.Management.Pooling
{
    public class Container<T>
    {
        private readonly List<Clone<T>> _clones;
        private readonly Dictionary<T, Clone<T>> _container;

        private readonly InstantiateCallback<T> _callback;
        
        private int _index;

        public Container(int capacity, InstantiateCallback<T> callback)
        {
            _clones = new List<Clone<T>>(capacity);
            _container = new Dictionary<T, Clone<T>>(capacity);
            _callback = callback;

            // Initialize object containers
            InitializeContainers(capacity);
        }

        private void InitializeContainers(int capacity)
        {
            for (var i = 0; i < capacity; i++)
            {
                CreateClone();
            }
        }

        private Clone<T> CreateClone()
        {
            if (_callback is null)
            {
                return null;
            }

            var clone = new Clone<T>(_callback.Invoke());
            _clones.Add(clone);

            return clone;
        }

        public T Component
        {
            get
            {
                Clone<T> clone = null;

                var count = _clones.Count;
                for (var i = 0; i < count; i++)
                {
                    _index++;

                    if (_index > _clones.Count - 1)
                    {
                        _index = 0;
                    }

                    if (_clones[_index].IsUsing)
                    {
                        continue;
                    }

                    clone = _clones[_index];
                    
                    break;
                }

                // If all object is used, create new clone.
                clone ??= CreateClone();

                // Set instance in clone used and add pool
                clone.Use();
                _container.Add(clone.Component, clone);

                return clone.Component;
            }
        }

        public void Release(T key)
        {
            if (_container.TryGetValue(key, out var clone) == false)
            {
                return;
            }
            
            clone.Release();
            _container.Remove(key);
        }

        public int ContainedCloneCount => _clones.Count;

        public int UsingCloneCount => _container.Count;
    }

    public delegate T InstantiateCallback<out T>();
}
