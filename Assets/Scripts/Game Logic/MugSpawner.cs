using System;
using UnityEngine;

namespace Game_Logic {
    public class MugSpawner : MonoBehaviour {
        public GameManager gameManager;
        
        public GameObject Mug;

        public static bool MugSpawned;

        private void Start() {
            MugSpawned = true;
        }


        void FixedUpdate() {
            if (gameManager.sessionAlive) {
                if (MugSpawned) return;
                Instantiate(Mug, Mug.transform.position, Quaternion.Euler(-90, 180, -20));
                MugSpawned = true;
            }
        }
    }
}