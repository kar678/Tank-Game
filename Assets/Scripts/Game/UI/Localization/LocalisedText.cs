using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Cogwheel.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalisedText : MonoBehaviour
    {
        TextMeshProUGUI textField;

        public LocalisedString LocalisedString;

        // Start is called before the first frame update
        void Start()
        {
            textField = GetComponent<TextMeshProUGUI>();
            textField.text = LocalisedString.Value;
        }
    }
}
