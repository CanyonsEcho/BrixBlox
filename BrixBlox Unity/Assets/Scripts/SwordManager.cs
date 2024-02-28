using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SwordManager
{
    public class SwordManager : MonoBehaviour, IPointerClickHandler
    {
        private Image image;

        public bool isEquipped;
        public MeshRenderer sword;

        void Start()
        {
            image = gameObject.GetComponent<Image>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isEquipped)
            {
                image.color = new Color(0, 0, 0, 150 / 255f);
                sword.enabled = false;
                isEquipped = false;
            }
            else
            {
                image.color = new Color(0, 1, 0, 150 / 255f);
                sword.enabled = true;
                isEquipped = true;
            }
        }
    }
}