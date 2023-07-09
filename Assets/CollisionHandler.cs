using System;
using UnityEngine;
using System.Collections;

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
                gameObject.GetComponentInChildren<Animator>().SetBool("IsDead", true);
            }
            PlayVisualEffect();
            StartCoroutine(SlimeDeath());
        }
    }

    IEnumerator SlimeDeath()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
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
