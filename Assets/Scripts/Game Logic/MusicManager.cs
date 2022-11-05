using UnityEngine;

namespace Game_Logic {
    public class MusicManager : MonoBehaviour {
        public static MusicManager Instance;
    
        void Start() {

            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }

    
        void Update() {
        }
    }
}