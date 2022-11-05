using TMPro;
using UnityEngine;

namespace Game_Logic {
    public class ScoreCounter : MonoBehaviour {
        public GameObject newHighscore;
        public GameObject currentHighscore;
        
        public TextMeshProUGUI highscoreText;
        private int highscore;
        private static readonly int HighscoreReached = Animator.StringToHash("highscoreReached");
        
        private TextMeshProUGUI scoreText;
        private int score;

        private void Start() {
            highscore = PlayerPrefs.GetInt("highscore", 0);
            highscoreText.text = highscore.ToString();
            
            scoreText = GetComponent<TextMeshProUGUI>();
        }

        public void OnCoffeeSold() {
            score++;
            scoreText.text = score.ToString();

            if (score > highscore) {
                var anim = newHighscore.GetComponent<Animator>();
                newHighscore.SetActive(true);
                anim.SetBool(HighscoreReached, true);
            }
        }

        public void OnGameLost() {
            if (score > highscore) {
                PlayerPrefs.SetInt("highscore", score);
            }
        }

        public void EnableScore() {
            gameObject.SetActive(true);
            
            currentHighscore.SetActive(true);
        }
    }
}