using UnityEngine;

public class GrannyHealth : MonoBehaviour
{
    public int maxHealth = 3;                    // Maximum health of the Granny
    public int currentHealth;                    // Current health of the Granny
    public Transform respawnLocation;            // Respawn location for the Granny

    //private Vector3 initialPosition;              // Initial position of the Granny

    private void Start()
    {
        currentHealth = maxHealth;
        //initialPosition = respawnLocation.position;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        {
            // Play hurt animation or effects
        }
    }

    private void Die()
    {
        Debug.Log("Granny Died");

        // Reset Granny's position to the respawn location
        transform.position = respawnLocation.position;

        // Reset Granny's health to maxHealth
        currentHealth = maxHealth;

        // Perform any additional actions upon respawn

        // Example: Restart Granny's movement or other behaviors
        
        /*GrannyMovement grannyMovement = GetComponent<GrannyMovement>();
        if (grannyMovement != null)
        {
            grannyMovement.RestartMovement();
        }
        */
    }
}