using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMovement : MonoBehaviour
{
    public CharacterController controller;
    public GameObject sword;
    public GameObject swordTip;
    public Camera cam;


    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

        sword.transform.position = worldPos;
    }
}
