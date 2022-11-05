using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game_Logic {
    public class ClientScaleCounter : MonoBehaviour {
        public GameManager gameManager;
        public GameObject scaleFrame;
        
        private RectTransform scale;
        private float clientSatisfaction;

        public UnityEvent gameLost;

        void Start() {
            scale = GetComponent<RectTransform>();

            clientSatisfaction = 820;

            StartCoroutine(ReduceSatisfactionByTime());
        }

        private void Update() {
            if (!gameManager.sessionAlive) return;
            if (clientSatisfaction > 820) clientSatisfaction = 820;
            scale.sizeDelta = new Vector2(clientSatisfaction, 80f);

            if (Math.Abs(clientSatisfaction) < 0.001f || clientSatisfaction < 0) {
                Debug.Log("Game lost");
                gameLost.Invoke();
            }
        }

        private IEnumerator ReduceSatisfactionByTime() {
            yield return new WaitUntil(() => gameManager.sessionAlive);
            
            while (clientSatisfaction > 0) {
                clientSatisfaction -= 3.2f;
                yield return new WaitForSeconds(0.2f);
            }
        }

        public void CountProportionPoints(float liquidY, float lineY) {
            const float delta = 0.08f;

            if (Math.Abs(liquidY - lineY) <= delta) {
                clientSatisfaction += 16f;
            }
            else {
                var length = Math.Abs(liquidY - lineY) * 50;
                clientSatisfaction -= length;
            }
        }

        public void AddSatisfaction() {
            clientSatisfaction += 10f;
        }

        public void EnableScale() {
            scaleFrame.SetActive(true);
        }
    }
}