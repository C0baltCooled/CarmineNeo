using UnityEngine;
using UnityEngine.Events;

namespace Harpnet.Carmine {
    public class Health : MonoBehaviour {
        public float maxHealth = 10.0f;
        public float criticalHealthRatio = 0.3f;

        public UnityAction<float, GameObject> OnDamage;
        public UnityAction<float> OnHealed;
        public UnityAction OnDie;

        public float currentHealth { get; set; }
        public bool invincible { get; set; }
        public bool canPickup() => currentHealth < maxHealth;

        public float getRatio() => currentHealth / maxHealth;
        public bool isCritical() => getRatio() <= criticalHealthRatio;

        private bool m_IsDead;

        private void Start() {
            currentHealth = maxHealth;
        }

        public void Heal(float healAmount) {
            float healthBefore = currentHealth;
            currentHealth += healAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0.0f, maxHealth);

            // Call OnHeal Action
            float trueHealAmount = currentHealth - healthBefore;
            if(trueHealAmount > 0.0f)
                OnHealed?.Invoke(trueHealAmount);
        }

        public void TakeDamage(float damage, GameObject damageSource) {
            if(invincible)
                return;

            float healthBefore = currentHealth;
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0.0f, maxHealth);

            // Call OnDamage Action
            float trueDamageAction = healthBefore - currentHealth;
            if(trueDamageAction > 0.0f)
                OnDamage?.Invoke(trueDamageAction, damageSource);

            CheckDead();
        }

        public void Kill() {
            currentHealth = 0.0f;

            // Call OnDamage Action
            OnDamage?.Invoke(maxHealth, null);

            CheckDead();
        }

        private void CheckDead() {
            if(m_IsDead)
                return;

            // Call OnDie Action
            if(currentHealth <= 0.0f) {
                m_IsDead = true;
                OnDie?.Invoke();
            }
        }
    }
}