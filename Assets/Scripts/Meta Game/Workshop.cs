using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Meta_Game {
    public class Workshop : MonoBehaviour {
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private TextMeshProUGUI interiorShopHeader;
        [SerializeField] private GameObject[] interiorShopPanels;
        [SerializeField] private MoneyShower money;
        [SerializeField] private AudioSource purchaseSound;
        [SerializeField] private GameObject workshopButton;

        public InteriorItem[] allItems;
        
        private InteriorItem[] currentItems;

        private void Start() {

            if (PlayerPrefs.GetInt("first game", 0) == 0) {
                SetFirstItems();
            }

            currentItems = new InteriorItem[11];
        
            var x = 0;
            foreach (var item in allItems) {
                if (PlayerPrefs.GetInt(item.itemName + "_cur", 0) != 1) continue;
                
                currentItems[x] = item;
                item.button.gameObject.SetActive(false);
                item.priceText.gameObject.SetActive(false);
                x++;
            }
        
        
            InitializeBoughtItems();

            InitializeCurrentItems();
        
            gameObject.SetActive(false);
        }

        private void SetFirstItems() {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money", 0) + 5000);

            for (var i = 0; i < 22; i += 2) {
                PlayerPrefs.SetInt(allItems[i].itemName, 1);
                PlayerPrefs.SetInt(allItems[i].itemName + "_cur", 1);
                allItems[i].bought = true;
            }

            PlayerPrefs.SetInt("first game", 1);
        }

        private void InitializeBoughtItems() {
            foreach (var t in allItems) {
                if (t.bought) {
                    t.priceText.SetActive(false);
                    t.button.GetComponentInChildren<Text>().text = "Choose";
                }
                else {
                    t.priceText.SetActive(true);
                    t.button.GetComponentInChildren<Text>().text = "Buy";
                }
            }
        }

        private void InitializeCurrentItems() {
            for (var i = 0; i < 11; ++i) {
                currentItems[i].gameObject.SetActive(true);
            }
        }

        public void ChooseInteriorItem(int index) {
            if (allItems[index].bought) {
                var newItem = allItems[index];

                var oldItem = currentItems[newItem.category];
                oldItem.gameObject.SetActive(false);
                currentItems[oldItem.category].button.gameObject.SetActive(true);
                PlayerPrefs.SetInt(oldItem.itemName + "_cur", 0);

                newItem.gameObject.SetActive(true);
                allItems[index].button.gameObject.SetActive(false);

                PlayerPrefs.SetInt(newItem.itemName + "_cur", 1);
                currentItems[newItem.category] = newItem;
            }
        }

        public void BuyInteriorItem(int index) {
            if (!allItems[index].bought && money.EnoughMoney()) {
                allItems[index].bought = true;

                var item = allItems[index];
                PlayerPrefs.SetInt(item.itemName, 1);
                allItems[index].priceText.SetActive(false);
                allItems[index].button.GetComponentInChildren<Text>().text = "Choose";

                purchaseSound.Play();
                money.UpdateMoney(-1500);
            }
        }

        public void OpenWorkshop() {
            if (gameObject.activeInHierarchy) {
                gameObject.SetActive(false);
            }
            else {
                gameObject.SetActive(true);
            }
        }

        public void OpenInteriorShopPanel(int index) {
            gameObject.SetActive(false);
            workshopButton.SetActive(false);

            mainPanel.SetActive(true);

            interiorShopPanels[index].SetActive(true);
            interiorShopHeader.text = interiorShopPanels[index].name;
        }

        public void BackToWorkshop() {
            foreach (var interiorShopPanel in interiorShopPanels) {
                interiorShopPanel.SetActive(false);
            }
        
            mainPanel.SetActive(false);
        
            workshopButton.SetActive(true);
            gameObject.SetActive(true);
        }

    }
}