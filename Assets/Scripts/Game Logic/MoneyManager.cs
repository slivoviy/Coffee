using System.Collections;
using TMPro;
using UnityEngine;

namespace Game_Logic {
    public class MoneyManager : MonoBehaviour {
        public TextMeshProUGUI moneyText;
        public TextMeshProUGUI moneyAnimation;
        private static readonly int MoneyGained = Animator.StringToHash("moneyGained");
        public TextMeshProUGUI earnedMoneyText;
        
        private int money;
        public int newMoney;

        void Start() {
            
            money = PlayerPrefs.GetInt("money", 0);

            moneyText.text = "<sprite=0> " + money;

        }

        public void EarnMoney(int liquidsAmount) {
            int earnedMoney = Random.Range(0, 8) + liquidsAmount;

            money += earnedMoney;
            newMoney += earnedMoney;

            StartCoroutine(MoneyAnimation(earnedMoney));
        }

        private IEnumerator MoneyAnimation(int eMoney) {
            moneyAnimation.text = "<sprite=0> " + eMoney;
            var anim = moneyAnimation.GetComponent<Animator>();
            anim.SetTrigger(MoneyGained);
            
            yield return new WaitForSeconds(0.5f);
            
            yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
            
            anim.ResetTrigger(MoneyGained);
            moneyText.text = "<sprite=0> " + money;
        }

        public void OnGameLost() {
            PlayerPrefs.SetInt("money", money);

            earnedMoneyText.text = "<sprite=0> " + newMoney;
        }
    }
}