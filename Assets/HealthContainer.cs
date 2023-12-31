using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthContainer : MonoBehaviour
{
    public HealthContainer next;

    [Range(0, 1)] float fill;
    [SerializeField] Image fillImage;

    public void SetHeart(float count)
    {
        fill = count;
        fillImage.fillAmount = fill;
        count--;
        if (next != null)
        {
            next.SetHeart(count);
        }
    }
}
