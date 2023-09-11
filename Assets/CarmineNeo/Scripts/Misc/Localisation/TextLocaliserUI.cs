using UnityEngine;
using TMPro;

namespace CarmineNeo.Misc.Localisation {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextLocaliserUI : MonoBehaviour {
        TextMeshProUGUI textField;

        public string key;

        private void Awake() {
            textField = GetComponent<TextMeshProUGUI>();
            string value = LocalisationSystem.GetLocalisedValue(key);
            textField.text = value;
        }
    }
}