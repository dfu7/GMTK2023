using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMovement : MonoBehaviour
{
    public CharacterController player;
    public Transform swordTip;
    public Camera cam;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 camPos = cam.ScreenToWorldPoint(mousePos);

        Vector3 pos = swordTip.position;
        pos.y = camPos.y;

        swordTip.position = pos;
    }
}
