using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class ToTheCafeLoader : MonoBehaviour
    {
        public void ToTheCafe() {
            Time.timeScale = 1f;
            
            SceneManager.LoadScene("Coffee Shop Scene");
        }
    }
}
