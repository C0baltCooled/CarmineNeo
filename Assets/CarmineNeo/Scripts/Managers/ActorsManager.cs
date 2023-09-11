using System.Collections.Generic;
using UnityEngine;

namespace Harpnet.Carmine {
    public class ActorsManager : MonoBehaviour {
        public List<Actor> actors { get; private set; }
        public GameObject player { get; private set; }

        public void SetPlayer(GameObject setPlayer) => player = setPlayer;

        private void Awake() {
            actors = new List<Actor>();
        }
    }
}