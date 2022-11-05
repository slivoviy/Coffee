using UnityEngine;

namespace UI {
    public class ButtonActivator : MonoBehaviour
    {
        public GameObject Button;
        public void DeactivateButton()
        {
            Button.SetActive(false);
        }
        public void ActivateButton()
        {
            Button.SetActive(true);
        }
    }
}
