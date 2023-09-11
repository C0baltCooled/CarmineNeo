using UnityEngine;

namespace Harpnet.Carmine {
    public class Actor : MonoBehaviour {
        public int affiliation;

        private ActorsManager m_ActorsManager;

        private void Start() {
            // Register actor
            if(!m_ActorsManager.actors.Contains(this))
                m_ActorsManager.actors.Add(this);
        }

        private void OnDestroy() {
            // Unregister actor
            if(m_ActorsManager)
                m_ActorsManager.actors.Remove(this);
        }
    }
}