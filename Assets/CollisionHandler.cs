using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public GameObject visualEffect; // Assign the visual effect prefab in the Inspector

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            PlayVisualEffect();
            Destroy(gameObject);
        }
    }

    private void PlayVisualEffect()
    {
        if (visualEffect != null)
        {
            Instantiate(visualEffect, transform.position, Quaternion.identity);
        }
    }
}
