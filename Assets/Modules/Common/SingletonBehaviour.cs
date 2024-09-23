namespace Voyage.Common
{
    using UnityEngine;

    public abstract class SingletonBehaviour<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T ms_Instance;

        public static T GetInstance() => ms_Instance;

        protected virtual void Awake()
        {
#if DEBUG
            if (!ReferenceEquals(ms_Instance, null))
                Debug.LogError("Singleton has instantiated more than 2.");
#endif
            ms_Instance = this as T;
        }

        protected void OnDestroy()
        {
            ms_Instance = null;
        }
    }
}