using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public static HealthBar instance;

    [SerializeField] GameObject heartContainerPrefab;

    [SerializeField] List<GameObject> heartContainers;
    int totalHearts;
    float currentHearts;
    HealthContainer currentContainer;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        heartContainers = new List<GameObject>();

    }
    //ZeldaHealthBar.instance.SetupHearts(valueIn);
    public void SetupHearts(int heartsIn)
    {
        heartContainers.Clear();
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        totalHearts = heartsIn;
        currentHearts = (float)totalHearts;

        for (int i = 0; i < totalHearts; i++)
        {
            GameObject newHeart = Instantiate(heartContainerPrefab, transform);
            heartContainers.Add(newHeart);
            if (currentContainer != null)
            {
                currentContainer.next = newHeart.GetComponent<HealthContainer>();
            }
            currentContainer = newHeart.GetComponent<HealthContainer>();
        }
        currentContainer = heartContainers[0].GetComponent<HealthContainer>();
    }
    //ZeldaHealthBar.instance.SetCurrentHealth(valueIn);
    public void SetCurrentHealth(float health)
    {
        currentHearts = health;
        currentContainer.SetHeart(currentHearts);

    }
    //ZeldaHealthBar.instance.AddHearts(valueIn);
    public void AddHearts(float healthUp)
    {
        currentHearts += healthUp;
        if (currentHearts > totalHearts)
        {
            currentHearts = (float)totalHearts;
        }
        currentContainer.SetHeart(currentHearts);
    }
    //ZeldaHealthBar.instance.RemoveHearts(valueIn);
    public void RemoveHearts(float healthDown)
    {
        currentHearts -= healthDown;
        if (currentHearts < 0)
        {
            currentHearts = 0f;
        }
        currentContainer.SetHeart(currentHearts);
    }
    //ZeldaHealthBar.instance.AddContainer(valueIn);
    public void AddContainer()
    {
        GameObject newHeart = Instantiate(heartContainerPrefab, transform);
        currentContainer = heartContainers[heartContainers.Count - 1].GetComponent<HealthContainer>();
        heartContainers.Add(newHeart);

        if (currentContainer != null)
        {
            currentContainer.next = newHeart.GetComponent<HealthContainer>();
        }

        currentContainer = heartContainers[0].GetComponent<HealthContainer>();
        totalHearts++;
        currentHearts = totalHearts;
        SetCurrentHealth(currentHearts);
    }
}
