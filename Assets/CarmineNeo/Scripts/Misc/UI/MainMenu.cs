using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CarmineNeo.UI {
    public class MainMenu : MonoBehaviour {
        /* Background */
        [SerializeField] private TextMeshProUGUI versionText;

        /* Main Menu */
        [Header("Main Menu")]
        [SerializeField] private GameObject mainMenu;

        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI playerText;
        [SerializeField] private TextMeshProUGUI statusText;

        /* Name */
        private new string name = null;

        /* Player Character */
        [SerializeField, Tooltip("Same order as the enum")] private Sprite[] characterIcons;
        [SerializeField] private Image[] characterImages;

        private Character selectedCharacter = Character.Sarah;

        /* Server Status */
        private bool serverStatus = false;
        private string statusString = null;

        private bool result;

        private void Awake() {
            // Set Version Text
            if(versionText != null)
                versionText.text = "Version: " + Application.version;

            SetupMainMenu();
        }

        private void Start() {
            // Open Main Menu (So it ZOOM)
            mainMenu.SetActive(true);
        }

        private void SetupMainMenu() {
            /* Name */
            name = ReadString("PlayerName", ref result);
            if(result)
                nameText.text = "Name: " + name;
            else
                nameText.text = "Name: CardboardPlayer";

            /* Player */
            selectedCharacter = (Character)ReadInt("PlayerCharacter", ref result);
            if(!result)
                selectedCharacter = Character.Sarah;

            // Change Player Icons
            for(int i = 0; i < characterImages.Length; ++i) {
                characterImages[i].sprite = characterIcons[(int)selectedCharacter];
            }

            // Change Player Text
            playerText.text += " " + (Character)selectedCharacter;

            /* Server Status */
            if(serverStatus)
                statusString = Misc.Localisation.LocalisationSystem.GetLocalisedValue("gui.network.online");
            else
                statusString = Misc.Localisation.LocalisationSystem.GetLocalisedValue("gui.network.offline");

            statusText.text = statusString;
        }

        /* Read an Int from the PlayerPrefs */
        private int ReadInt(string key, ref bool result) {
            if(PlayerPrefs.HasKey(key)) {
                result = true;
                return PlayerPrefs.GetInt(key);
            } else {
                result = false;
                return 0;
            }
        }

        /* Read a Float from the PlayerPrefs */
        private float ReadFloat(string key, ref bool result) {
            if(PlayerPrefs.HasKey(key)) {
                result = true;
                return PlayerPrefs.GetFloat(key);
            } else {
                result = false;
                return 0f;
            }
        }

        /* Read a String from the PlayerPrefs */
        private string ReadString(string key, ref bool result) {
            if(PlayerPrefs.HasKey(key)) {
                result = true;
                return PlayerPrefs.GetString(key);
            } else {
                result = false;
                return null;
            }
        }

        private enum Character {
            Amber,
            Ashley,
            Bae,
            Emily,
            Sarah,
            Savannah
        }
    }
}