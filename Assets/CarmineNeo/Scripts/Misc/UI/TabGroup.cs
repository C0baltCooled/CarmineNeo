using System.Collections.Generic;
using UnityEngine;

namespace Harpnet.Carmine {
    public class TabGroup : MonoBehaviour {
        [SerializeField] private List<TabButton> tabButtons;
        [SerializeField] private Color tabIdle;
        [SerializeField] private Color tabHover;
        [SerializeField] private Color tabActive;
        [SerializeField] private TabButton selectedTab;
        [SerializeField] private List<GameObject> objectsToSwap;

        public void Subscribe(TabButton button) {
            if(tabButtons == null)
                tabButtons = new List<TabButton>();

            tabButtons.Add(button);
        }

        public void OnTabEnter(TabButton button) {
            ResetTabs();
            if(selectedTab == null || button != selectedTab)
                button.background.color = tabHover;
        }

        public void OnTabExit(TabButton button) {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button) {
            selectedTab = button;
            ResetTabs();
            button.background.color = tabActive;
            int index = button.transform.GetSiblingIndex();
            for(int i = 0; i < objectsToSwap.Count; ++i) {
                if(i == index)
                    objectsToSwap[i].SetActive(true);
                else
                    objectsToSwap[i].SetActive(false);
            }
        }

        public void ResetTabs() {
            foreach(TabButton button in tabButtons) {
                if(selectedTab != null && button == selectedTab)
                    continue;
                button.background.color = tabIdle;
            }
        }
    }
}