using UnityEngine;

public class GrannyHealth : MonoBehaviour
{
    public int maxHealth = 3;                    // Maximum health of the Granny
    public int currentHealth;                    // Current health of the Granny
    public Transform respawnLocation;            // Respawn location for the Granny

    public GameObject AttackEffect;
    public GameObject DeathEffect;

    AudioSource g_audioSource;
    public AudioClip dingClip;

    private void Start()
    {
        g_audioSource = gameObject.AddComponent<AudioSource>();
        currentHealth = maxHealth;
        HealthBar.instance.SetupHearts(3);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        HealthBar.instance.RemoveHealth(damage);

        g_audioSource.clip = dingClip;
        g_audioSource.Play();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            GameObject attackInstance = Instantiate(AttackEffect, transform.position, Quaternion.identity);
            Destroy(attackInstance, 1f);


            // Play hurt animation or effects
        }
    }

    private void Die()
    {
        Debug.Log("Granny Died");

        GameObject DeathInstance = Instantiate(DeathEffect, transform.position, Quaternion.identity);
        Destroy(DeathInstance, 1f);

        // Start the respawn coroutine
        StartCoroutine(RespawnAfterDelay());
    }
    private System.Collections.IEnumerator RespawnAfterDelay()
    {
            yield return new WaitForSeconds(1f);

            // Reset Granny's position to the respawn location
            transform.position = respawnLocation.position;

            // Reset Granny's health to maxHealth
            currentHealth = maxHealth;
        HealthBar.instance.SetupHearts(maxHealth);
    }

}