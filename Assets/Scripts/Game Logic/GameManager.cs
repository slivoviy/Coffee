using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game_Logic {
    public class GameManager : MonoBehaviour {
        public GameObject lostPanel;
        public bool sessionAlive;
        public bool gameLost;
        public float difficulty;

        private void Start() {
            StartCoroutine(RaiseDifficulty());
        }

        public void StopSession() {
            sessionAlive = false;
        }

        public void StartSession() {
            sessionAlive = true;
        }

        public void OnGameLost() {
            sessionAlive = false;
            gameLost = true;

            Time.timeScale = 0f;
            
            lostPanel.SetActive(true);
            lostPanel.GetComponent<Animator>().SetTrigger("gameLost");
        }

        private IEnumerator RaiseDifficulty() {
            yield return new WaitUntil(() => sessionAlive);

            while (!gameLost && Math.Abs(difficulty - 0.2f) > 0.0001f) {
                yield return new WaitForSeconds(5f);

                difficulty += 0.01f;
            }
        }
    }
}