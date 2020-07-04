using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

namespace Cogwheel.UI
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
    {
        public TabGroup tabGroup;
        public Image background;
        public TextMeshProUGUI text;

        public UnityEvent onTabSelected;
        public UnityEvent onTabDeselected;

        public void OnPointerClick(PointerEventData eventData)
        {
            tabGroup.OnTabSelected(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            background = GetComponent<Image>();
            text = GetComponentInChildren<TextMeshProUGUI>();
            tabGroup.Subscribe(this);
        }

        public void Select()
        {
            if (onTabSelected != null)
            {
                onTabSelected.Invoke();
            }
        }

        public void Deselect()
        {
            if (onTabDeselected != null)
            {
                onTabDeselected.Invoke();
            }
        }
    }
}
