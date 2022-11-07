using UnityEngine;
using UnityEngine.UI;

namespace Meta_Game {
    public class InteriorItem : MonoBehaviour {
        public GameObject priceText;
        public Button button;
        public int category;
        public string itemName;
        
        // [HideInInspector]
        public bool bought;

    }
}