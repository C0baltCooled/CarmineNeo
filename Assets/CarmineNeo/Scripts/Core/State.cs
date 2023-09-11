using System.Collections;

namespace Harpnet.Carmine {
    public abstract class State {
        public State() {

        }

        public virtual IEnumerator Start() {
            yield break;
        }

        public virtual IEnumerator Menu() {
            yield break;
        }

        public virtual IEnumerator Alive() {
            yield break;
        }

        public virtual IEnumerator Dead() {
            yield break;
        }

        public virtual IEnumerator Spectating() {
            yield break;
        }
    }
}