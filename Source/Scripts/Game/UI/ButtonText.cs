using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Cogwheel.UI
{
    public class ButtonText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Color normalColor = Color.black;
        public Color hoveredColor = Color.white;

        public TextMeshProUGUI text;
        public TextMeshProUGUI text2;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (text)
            {
                text.color = hoveredColor; //Or however you do your color
            }

            if(text2)
            {
                text2.color = hoveredColor; //Or however you do your color
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (text)
            {
                text.color = normalColor; //Or however you do your color
            }

            if(text2)
            {
                text2.color = normalColor; //Or however you do your color
            }
        }
    }
}
