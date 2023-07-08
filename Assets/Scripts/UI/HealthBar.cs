using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public static HealthBar instance;

    [SerializeField] GameObject healthContainerPrefab;
    [SerializeField] List<GameObject> healthContainers;

    int totalHealth;
    float currentHealth;
    HealthContainer currentContainer;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        healthContainers = new List<GameObject>();
    }

    public void SetupHearts(int healthIn)
    {
        healthContainers.Clear();
        for(int i = transform.childCount -1; i >=0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        totalHealth = healthIn;
        currentHealth = (float)totalHealth;

        for (int i = 0; i < totalHealth; i++)
        {
            GameObject newHealth = Instantiate(healthContainerPrefab, transform);
            healthContainers.Add(newHealth);
            if(currentContainer != null)
            {
                currentContainer.next = newHealth.GetComponent<HealthContainer>();
            }
            currentContainer = newHealth.GetComponent<HealthContainer>();
        }
        currentContainer = healthContainers[0].GetComponent<HealthContainer>();
    }
    public void SetCurrentHealth(float health)
    {
        currentHealth = health;
        currentContainer.SetHealth(currentHealth);
    }

    public void AddHealth(float healthUp)
    {
        currentHealth += healthUp;
        if (currentHealth > healthUp)
        {
            currentHealth = (float)totalHealth;
        }
        currentContainer.SetHealth(currentHealth);
    }

    public void RemoveHealth(float healthDown)
    {
        currentHealth -= healthDown;
        if (currentHealth < 0)
        {
            currentHealth = 0f;
        }
        currentContainer.SetHealth(currentHealth);
    }

    public void AddContainer()
    {
        GameObject newHeart = Instantiate(healthContainerPrefab, transform);
        currentContainer = healthContainers[healthContainers.Count - 1].GetComponent<HealthContainer>();
        healthContainers.Add(newHeart);

        if (currentContainer != null)
        {
            currentContainer.next = newHeart.GetComponent<HealthContainer>();
        }

        currentContainer = healthContainers[0].GetComponent<HealthContainer>();

        totalHealth++;
        currentHealth = totalHealth;
        SetCurrentHealth(currentHealth);
    }
}
