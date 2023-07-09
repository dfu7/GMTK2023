using UnityEngine;

public class SlimeHealth : MonoBehaviour
{
    public delegate void DeathAction(GameObject slime);
    public event DeathAction OnDeath;   // Event triggered when the slime dies

    public void TakeDamage(int damage)
    {
        Die();
    }

    private void Die()
    {
        // Trigger the OnDeath event
        OnDeath?.Invoke(gameObject);

        // Destroy the slime game object
        Destroy(gameObject);
    }
}
