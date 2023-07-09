using System;
using UnityEngine;
using System.Collections;

public class CollisionHandler : MonoBehaviour
{
    public static event Action SlimeKilled = delegate { };

    public GameObject visualEffect; // Assign the visual effect prefab in the Inspector

    public GameObject AttackEffect;

    AudioSource audioSource;
    public AudioClip audioClip;

    private void OnTriggerEnter(Collider collision)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.5f;

        if (collision.gameObject.CompareTag("Sword"))
        {
            PlayHitEffect();

            if (!gameObject.CompareTag("Unbreakable"))
            {
                PlayVisualEffect();
            }
            else
            {
                // unbreakable
                audioSource.clip = audioClip;
                audioSource.Play();
                return;
            }

            if (gameObject.CompareTag("Slime"))
            {
                SlimeKilled();
                gameObject.GetComponentInChildren<Animator>().SetBool("IsDead", true);
                //StartCoroutine(SlimeDeath());
            }

            StartCoroutine(DestroyAfterAudio());
        }

    }

    IEnumerator DestroyAfterAudio()
    {
        if (!audioClip)
        {
            Destroy(gameObject);
        }

        audioSource.clip = audioClip;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(gameObject);
    }

    IEnumerator SlimeDeath()
    {
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    private void PlayHitEffect()
    {
        if (AttackEffect != null)
        {
            GameObject attackInstance = Instantiate(AttackEffect, transform.position, Quaternion.identity);
            Destroy(attackInstance, 1f);
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
