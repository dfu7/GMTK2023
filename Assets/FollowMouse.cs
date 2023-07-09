using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowMouse : MonoBehaviour
{
    public Transform mainCamera;
    public CinemachineFreeLook freeLookCam;

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
    Plane planeY;
    Vector3 screenPos;
    Vector3 worldPos;
    Vector3 newPos;

    public GameObject go;
    public Rigidbody goRig;
    public float MAXswordRadius = 5f;
    public float MINswordRadius = 1f;
    Vector3 lastPos = Vector3.zero;
    public float flightSpeedThreshold = 500f;
    public float flightSpeedMAX = 600f;
    public float flightSwingDistanceThreshold = 10f;
    public float flightStopSpeed = 1f;

    bool playerLaunching = false;
    public FixedJoint swordJoint;

    public float camXspeed = 700f;
    public float camYspeed = 5f;

    private void Start()
    {
        qTo = transform.rotation;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {

        // moving
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


        if (Input.GetMouseButton(1))
        {
            freeLookCam.m_YAxis.m_MaxSpeed = camYspeed;
            freeLookCam.m_XAxis.m_MaxSpeed = camXspeed;
            Debug.Log("RIGHT LCIKGING");
        }
        else
        {
            freeLookCam.m_YAxis.m_MaxSpeed = 0;
            freeLookCam.m_XAxis.m_MaxSpeed = 0;

            // other stuff
            planeY = new Plane(Vector3.down, grannyMidPoint.position.y);

            screenPos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(screenPos);

            if (planeY.Raycast(ray, out float distanceY))
            {
                worldPos = ray.GetPoint(distanceY);
            }

            // cursor position
            newPos = worldPos;
            float newPosDis = Vector3.Distance(newPos, grannyMidPoint.position);
            Vector3 swordDir = (worldPos - grannyMidPoint.position).normalized;

            // if the cursor position is out of radius, set it to be set to the max in the direction of the cursor
            if (newPosDis > MAXswordRadius)
            {
                // direction the sword should point according to the mouse
                newPos = grannyMidPoint.position + swordDir * MAXswordRadius;
            }

            if (newPosDis < MINswordRadius)
            {
                newPos = grannyMidPoint.position + swordDir * MINswordRadius;
            }

            go.transform.position = new Vector3(newPos.x, grannyMidPoint.position.y, newPos.z);

            // rotate sword
            go.transform.rotation = Quaternion.LookRotation(swordDir);
        }
    }
}
