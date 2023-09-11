using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Harpnet.Carmine {
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler {
        [SerializeField] private TabGroup tabGroup;
        [HideInInspector] public Image background;

        private void Start() {
            background = GetComponent<Image>();
            tabGroup.Subscribe(this);
        }

        public void OnPointerClick(PointerEventData eventData) {
            tabGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData) {
            tabGroup.OnTabExit(this);
        }
    }
}