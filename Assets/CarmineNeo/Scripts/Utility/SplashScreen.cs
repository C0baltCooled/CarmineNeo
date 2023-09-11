using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Harpnet.Carmine {
    public class SplashScreen : MonoBehaviour {
        public void Start() {
            StartCoroutine(Splash());
        }

        private IEnumerator Splash() {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}