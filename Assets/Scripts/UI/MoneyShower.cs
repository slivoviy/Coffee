using TMPro;
using UnityEngine;

namespace UI {
    public class MoneyShower : MonoBehaviour {
        private TextMeshProUGUI moneyText;

        void Start() {
            moneyText = GetComponent<TextMeshProUGUI>();
            
            moneyText.text = "<sprite=0> " + PlayerPrefs.GetInt("money", 0);
        }

        public void UpdateMoney(int money) {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) + money);
            
            moneyText.text = "<sprite=0> " + PlayerPrefs.GetInt("money", 0);
        }

        public bool EnoughMoney() {
            return PlayerPrefs.GetInt("money", 0) > 1500;
        }
    }
}