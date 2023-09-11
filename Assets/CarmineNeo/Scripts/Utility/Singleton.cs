using UnityEngine;

namespace Harpnet.Carmine {
    public class Singleton<T> : MonoBehaviour where T : Component {
        private static T _instance;
        public static T Instance {
            get {
                if(_instance == null) {
                    _instance = FindObjectOfType<T>();
                    if(_instance == null) {
                        GameObject newInstance = new GameObject();
                        _instance = newInstance.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake() {
            _instance = this as T;
        }
    }
}