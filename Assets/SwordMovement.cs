using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMovement : MonoBehaviour
{
    public Transform player;
    public Transform swordTip;
    public Camera cam;
    public float rotationSpeed = 5f;
    public float movementSpeed = 5f;
    public float distanceFromPlayer = 5f; // Distance in front of the player
    public float coneAngle = 180f; // Angle of the cone in degrees

    private Vector3 initialOffset;
    private Vector3 rotationOffset;
    private float rotationX;
    private float rotationY;

    private void Start()
    {
        // Calculate the initial offset between the object and the player
        initialOffset = transform.position - player.position;
        rotationOffset = transform.rotation.eulerAngles - player.rotation.eulerAngles;
    }

    private void Update()
    {
        // Get the mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Adjust the rotation angles based on mouse input
        rotationX += mouseX * rotationSpeed;
        rotationY -= mouseY * rotationSpeed;

        // Apply the rotation to the object relative to the player
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0f);
        Quaternion playerRotation = Quaternion.Euler(player.rotation.eulerAngles + rotationOffset);
        Vector3 desiredPosition = player.position + playerRotation * (rotation * initialOffset);

        // Calculate the direction from the player to the desired position
        Vector3 directionToDesiredPosition = (desiredPosition - player.position).normalized;

        // Calculate the forward direction within the cone
        Vector3 forwardDirection = player.forward;
        Vector3 playerToDesiredPosition = desiredPosition - player.position;
        playerToDesiredPosition.y = 0f;
        if (playerToDesiredPosition.magnitude > 0f)
        {
            forwardDirection = playerToDesiredPosition.normalized;
        }

        // Calculate the angle between the direction to the desired position and the forward direction
        float angle = Vector3.Angle(directionToDesiredPosition, forwardDirection);

        // Restrict the object's position within the cone angle
        Vector3 finalPosition;
        if (angle <= coneAngle * 0.5f)
        {
            finalPosition = desiredPosition;
        }
        else
        {
            Quaternion coneRotation = Quaternion.AngleAxis(coneAngle * 0.5f, Vector3.up);
            finalPosition = player.position + playerRotation * (coneRotation * (initialOffset.normalized * distanceFromPlayer));
        }

        // Move the object to the final position
        transform.position = finalPosition;

        // Calculate the movement direction relative to the player
        Vector3 movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        movementDirection = playerRotation * movementDirection;
        movementDirection.Normalize();

        // Move the object in the movement direction
        transform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);

        // Rotate the object to face the movement direction
        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
