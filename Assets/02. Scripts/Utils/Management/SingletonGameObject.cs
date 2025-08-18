using System.Text.RegularExpressions;
using UnityEngine;

namespace Utils.Management
{
    public class SingletonGameObject<T> : SingletonBehavior where T : MonoBehaviour
    {
        #region CONSTANT FIELD API

        private const string Suffix = " (Singleton)";

        private const string Pattern = "/([A-Z])(?=[A-Z][a-z])|([a-z])(?=[A-Z])/g";
        private const string Replacement = "$& ";

        #endregion

        private static T _instance;

        protected sealed override void Awake()
        {
            if (_instance is null)
            {
                _instance = this as T;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }

            base.Awake();
        }

        protected static T Instance
        {
            get
            {
                if (IsQuitting)
                {
                    return null;
                }

                if (_instance is null == false)
                {
                    return _instance;
                }

                // Search for all existing singleton instance.
                var instances = FindObjectsOfType<T>();

                var length = instances.Length;
                if (0 < length)
                {
                    if (length == 1)
                    {
                        return _instance = instances[0];
                    }

                    for (var i = 1; i < length; i++)
                    {
                        Destroy(instances[i]);
                    }

                    return _instance = instances[0];
                }

                // If it hasn't been created yet, create an instance.
                var gameObject = new GameObject();
                _instance = gameObject.AddComponent<T>();

                // Set singleton instance name.
                var name = Regex.Replace(typeof(T).Name, Pattern, Replacement);
                gameObject.name = name + Suffix;

                return _instance;
            }
        }
    }
}
