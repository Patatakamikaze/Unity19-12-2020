using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterHealth : MonoBehaviour {

    public float maxHealth;                                     //player's max health  
    public float freezeHealingTime;                             //Time to wait for healing again after the player is damaged    
    public float repeatDamagePeriod = 1f;                       //How frequently the player can be damaged.
    public bool healOverTime;
    [Range(0.1f, 100)]
    public float healingPointsPerSecond;                         //Amount of health points the player will recover by second  
    public UnityEvent OnDamage;
    public UnityEvent OnDeath;

    private float lastHitTime;                                   //The time at which the player was last hit.

    public float currentHealth { get; private set; }            //player's current health
    public bool isHurting { get; private set;}
    public bool isDead { get; private set; }


    private void Start() {
        currentHealth = maxHealth;
    }


    private void Update() {
        if(healOverTime)
            regenerateHealth();
    }

    public void hurt(int damage) {
        if (Time.time > lastHitTime + repeatDamagePeriod) {
            // ... take damage and reset the lastHitTime.
            StartCoroutine(takeDamage(damage));
            lastHitTime = Time.time;
        }
    }

    private void regenerateHealth() {
        if (Time.time > lastHitTime + repeatDamagePeriod + freezeHealingTime && !isHurting && !isDead) {
            if (currentHealth < maxHealth) {
                //regenerating health over time
                currentHealth += Time.deltaTime * healingPointsPerSecond;

                //Avoid the health to has a value over the maxHealth value
                if (currentHealth > maxHealth)
                    currentHealth = maxHealth;
            }
        }
    }

    private void death() {
        isDead = true;
        OnDeath.Invoke();
    }


    private IEnumerator takeDamage(int damage) {

        if (isHurting)
            yield return null;

        // Player is getting damage
        isHurting = true;
        // Reduce the player's health by enemy damage.
        currentHealth -= damage;

        OnDamage.Invoke();

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead) {
            currentHealth = 0;
            // ... it should die.
            death();
        }

        // Wait the repeatDamagePeriod until the player can receive damage again
        yield return new WaitForSeconds(repeatDamagePeriod);
        // Player is no longer taking damage
        isHurting = false;
    }
}
