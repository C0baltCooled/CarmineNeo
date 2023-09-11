using UnityEngine;

namespace Harpnet.Carmine {
    [RequireComponent(typeof(AudioSource))]
    public class WeaponController : MonoBehaviour {
        [SerializeField] private WeaponData weaponData;

        private int m_CurrentAmmo;
        private AudioSource m_AudioSource;

        public GameObject Owner { get; set; }

        private void Awake() {
            m_CurrentAmmo = weaponData.ammoMax;
            m_AudioSource = GetComponent<AudioSource>();
        }

        private void Update() {
            UpdateAmmo();
        }

        private void UpdateAmmo() {
            
        }
    }
}