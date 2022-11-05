using UnityEngine;

namespace Scripts_for_Objects {
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Coffee Recipe")]
    public class CoffeeRecipe : ScriptableObject {
        public string name;
    
        public Material[] liquids;
        public float[] proportions;
    }
}