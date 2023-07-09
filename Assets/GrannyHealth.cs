using UnityEngine;

public class GrannyHealth : MonoBehaviour
{
    public int maxHealth = 3;                    // Maximum health of the Granny
    public int currentHealth;                    // Current health of the Granny
    public Transform respawnLocation;            // Respawn location for the Granny

    public GameObject AttackEffect;
    public GameObject DeathEffect;

    private void Start()
    {
        currentHealth = maxHealth;
        HealthBar.instance.SetupHearts(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        HealthBar.instance.RemoveHealth(damage);

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

        // Reset Granny's health to maxHealth
        currentHealth = maxHealth;
        HealthBar.instance.AddHealth(maxHealth);

        // Start the respawn coroutine
        StartCoroutine(RespawnAfterDelay());
    }
    private System.Collections.IEnumerator RespawnAfterDelay()
    {
        // Store the initial scale
        Vector3 initialScale = transform.localScale;

        // Scale down the player gradually
        float duration = 0.5f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float scale = Mathf.Lerp(1f, 0.1f, t);
            transform.localScale = initialScale * scale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset the scale to the initial value
        transform.localScale = initialScale;

        // Reset Granny's position to the respawn location
        transform.position = respawnLocation.position;
    }

}