using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cogwheel.UI
{
    public class TabGroup : MonoBehaviour
    {
        #region Buttons
        [Header("Buttons")]
        public List<TabButton> tabButtons;
        public TabButton selectedTab;
        #endregion

        #region Options
        [Header("Options")]
        public bool useSprite = true;
        public bool changeTextColor = true;
        public bool startSelected = false;
        #endregion

        #region Button Sprites
        [Header("Button Sprites")]
        public Sprite tabIdle;
        public Sprite tabHover;
        public Sprite tabActive;
        #endregion

        #region Colors
        [Header("Colors")]
        public Color tabIdleColor = Color.white;
        public Color tabHoverColor = Color.red;
        public Color tabActiveColor = Color.blue;

        public Color textIdleColor = Color.black;
        public Color textHoverColor = Color.white;
        public Color textActiveColor = Color.white;
        #endregion

        public List<GameObject> objectsToSwap;

        public void Start()
        {
            if (selectedTab != null && startSelected)
            {
                selectedTab.Select();
                ResetTabs();

                if (useSprite)
                {
                    selectedTab.background.sprite = tabActive;
                }
                else
                {
                    selectedTab.background.color = tabActiveColor;
                }

                if (changeTextColor)
                {
                    selectedTab.text.color = textActiveColor;
                }

                int index = selectedTab.transform.GetSiblingIndex();

                for (int i = 0; i < objectsToSwap.Count; i++)
                {
                    if (i == index)
                    {
                        objectsToSwap[i].GetComponent<CanvasGroup>().alpha = 1;
                        objectsToSwap[i].GetComponent<CanvasGroup>().interactable = true;
                        objectsToSwap[i].GetComponent<CanvasGroup>().blocksRaycasts = true;
                    }
                    else
                    {
                        objectsToSwap[i].GetComponent<CanvasGroup>().alpha = 0;
                        objectsToSwap[i].GetComponent<CanvasGroup>().interactable = false;
                        objectsToSwap[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
                    }
                }
            }
            else if (startSelected != true && selectedTab != null)
            {
                selectedTab = null;
            }
        }

        public void Subscribe(TabButton button)
        {
            if (tabButtons == null)
            {
                tabButtons = new List<TabButton>();
            }

            tabButtons.Add(button);
        }

        public void OnTabEnter(TabButton button)
        {
            ResetTabs();
            if (selectedTab == null || button != selectedTab)
            {
                if (useSprite)
                {
                    button.background.sprite = tabHover;
                }
                else
                {
                    button.background.color = tabHoverColor;
                }

                if (changeTextColor)
                {
                    button.text.color = textHoverColor;
                }
            }
        }

        public void OnTabExit(TabButton button)
        {
            ResetTabs();
        }

        public void OnTabSelected(TabButton button)
        {
            if (selectedTab != null)
            {
                selectedTab.Deselect();
            }

            selectedTab = button;
            selectedTab.Select();

            ResetTabs();

            if (useSprite)
            {
                button.background.sprite = tabActive;
            }
            else
            {
                button.background.color = tabActiveColor;
            }

            if (changeTextColor)
            {
                button.text.color = textActiveColor;
            }

            int index = button.transform.GetSiblingIndex();

            for (int i = 0; i < objectsToSwap.Count; i++)
            {
                if (i == index)
                {
                    objectsToSwap[i].GetComponent<CanvasGroup>().alpha = 1;
                    objectsToSwap[i].GetComponent<CanvasGroup>().interactable = true;
                    objectsToSwap[i].GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
                else
                {
                    objectsToSwap[i].GetComponent<CanvasGroup>().alpha = 0;
                    objectsToSwap[i].GetComponent<CanvasGroup>().interactable = false;
                    objectsToSwap[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
            }
        }

        public void ResetTabs()
        {
            foreach (TabButton button in tabButtons)
            {
                if (useSprite)
                {
                    if (selectedTab != null && button == selectedTab) { continue; }
                    button.background.sprite = tabIdle;
                }
                else
                {
                    if (selectedTab != null && button == selectedTab) { continue; }
                    button.background.color = tabIdleColor;
                }

                if (changeTextColor)
                {
                    if (selectedTab != null && button == selectedTab) { continue; }
                    button.text.color = textIdleColor;
                }
            }
        }
    }
}
