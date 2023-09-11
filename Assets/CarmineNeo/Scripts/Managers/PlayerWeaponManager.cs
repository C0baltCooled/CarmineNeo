using System.Collections.Generic;
using UnityEngine;

namespace Harpnet.Carmine {
    public class PlayerWeaponManager : MonoBehaviour {
        public enum WeaponSwitchState {
            Up,
            Down,
            PutDownPrevious,
            PutUpNew
        }

        public int activeWeaponIndex { get; private set; }

        private WeaponSwitchState m_WeaponSwitchState;

        [SerializeField] private List<WeaponController> m_StartingWeapons = new List<WeaponController>();

        private void Start() {
            activeWeaponIndex = 0;
            m_WeaponSwitchState = WeaponSwitchState.Down;

            foreach(var weapon in m_StartingWeapons) {
                AddWeapon(weapon);
            }
        }

        private void AddWeapon(WeaponController weapon) {
            //if(HasWeapon(weapon) != null) {
                //return false;
            //}
        }
    }
}