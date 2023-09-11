using UnityEngine;

namespace Harpnet.Carmine {
    [System.Serializable]
    public class WeaponData {
        /* Core Values */
        public string weaponName = "New Weapon";        // What the weapon will be called in any text elements
        public Sprite weaponSprite = null;              // What the weapon icon will be for the HUD and the inventory
        public GameObject weaponPrefab = null;          // Weapon Prefab
        public CrosshairData crosshairData = null;

        /* Weapon Stats */
        public AudioClip sound;                         // What sound will be played when firing the weapon
        public int attackDelay = 0;                     // The delay between each attack
        public int damage = 0;                          // Damage of the weapon when hit
        public int critcalDamage = 0;                   // Damage when you hit a crit
        public int spread = 0;                          // Spread of the gun
        public int projectileSpeed = 0;                 // Speed of the projectiles
        public int recoilAmount = 0;                    // Recoil
        public int range = 0;                           // Max range of the weapon
        public int rays = 0;                            // How many projectiles to shoot
        public int hitPush = 0;                         // Velocity to push the shooter back
        public int ammoAdd = 0;                         // When picking up ammo, how much we should add
        public int ammoMax = 0;                         // The max amount of ammo allowed
        public int explosionRadius = 0;                 // The radius to do damage when the projectile explodes
        public int timeToExplode = 0;                   // Projectile life until it explodes

        /* Secondary Fire */
        public bool secondaryFire = false;              // Does the gun have a secondary fire?
        public int secondaryConsume = 0;                // How much ammo the gun consumes when using secondary fire
    }
}