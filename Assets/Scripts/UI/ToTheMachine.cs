using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI {
    public class ToTheMachine : MonoBehaviour {
        public void ToTheMach() {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) + 5000);
            SceneManager.LoadScene("Coffee Machine Scene");
        }

        public void DisableMusic() {
            GameObject.Find("Music").SetActive(false);
        }

        public void EnableMusic() {
            GameObject.Find("Music").SetActive(true);
        }
        
    }
}