using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public Transform mainCamera;

    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    Quaternion qTo;

    public float jumpHeight = 1f;
    public Transform ground_check;
    public float ground_distance = 0.4f;
    public LayerMask ground_mask;

    Vector3 velocity;
    bool isGrounded;
    float gravity = -9.10f;

    public Transform grannyMidPoint;
    Plane plane;
    Vector3 screenPos;
    Vector3 worldPos;

    public GameObject go;
    public float swordRadius = 5f;

    private void Start()
    {
        qTo = transform.rotation;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = Quaternion.Euler(0, mainCamera.eulerAngles.y, 0) * new Vector3(x, 0f, z);

        if (moveDir.magnitude >= 0.1f)
        {
            qTo = Quaternion.LookRotation(moveDir);
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, qTo, rotateSpeed * Time.deltaTime);

        // jumping
        isGrounded = Physics.CheckSphere(ground_check.position, ground_distance, ground_mask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        transform.Translate(velocity * Time.deltaTime);

        // other stuff
        /*
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Camera.main.nearClipPlane + 1;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Debug.DrawLine(transform.position, worldPos, Color.red);
        */

        plane = new Plane(Vector3.down, grannyMidPoint.position.y);

        screenPos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (plane.Raycast(ray, out float distance) )
        {
            worldPos = ray.GetPoint(distance);
        }

        Vector3 swordDir = (worldPos - grannyMidPoint.position).normalized;
        float swordDis = Vector3.Distance(worldPos, grannyMidPoint.position);

        go.transform.position = worldPos;
    }
}
