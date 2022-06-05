using UnityEngine;

namespace BCIT
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static bool _quitting = false;

        public static T Instance 
        {
            get {
                
                if(instance == null && !_quitting)
                {
                    Debug.LogError($"{typeof(T)} singleton is NULL");
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if(instance == null) instance = gameObject.GetComponent<T>();
            else if(instance.GetInstanceID() != GetInstanceID()){
                Destroy(gameObject);
                throw new System.Exception($"Instance of {GetType().FullName} already exists, removing {ToString()}");
            }
        }

        protected virtual void OnApplicationQuit() 
        {
            _quitting = true;
        }
    }
}