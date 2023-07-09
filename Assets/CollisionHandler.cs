using System;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public static event Action SlimeKilled = delegate { };

    public GameObject visualEffect; // Assign the visual effect prefab in the Inspector

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            if (gameObject.CompareTag("Slime"))
            {
                SlimeKilled();
            }
            PlayVisualEffect();
            Destroy(gameObject);
        }
    }

    private void PlayVisualEffect()
    {
        if (visualEffect != null)
        {
            GameObject vfxInstance = Instantiate(visualEffect, transform.position, Quaternion.identity);
            Destroy(vfxInstance, 1f);
        }
    }
}
