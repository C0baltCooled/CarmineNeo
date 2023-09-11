using UnityEngine;

namespace Harpnet.Carmine {
    public abstract class StateMachine : MonoBehaviour {
        protected State _state;

        public void SetState(State s) {
            _state = s;
            StartCoroutine(_state.Start());
        }
    }
}