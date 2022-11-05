using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class PauseButton : MonoBehaviour
    {
        public void Pause()
        {
            Time.timeScale = 0;
        }
    
        public void UnPause() {
            StartCoroutine(NoPause());
        }

        private IEnumerator NoPause() {
            yield return new WaitForSecondsRealtime(0.1f);
            Time.timeScale = 1;
        }

        public void GoHome() {
        
            Time.timeScale = 1;
            SceneManager.LoadScene("Coffee Machine Scene");
        }
    }
}
