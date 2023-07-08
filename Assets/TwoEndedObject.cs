using UnityEngine;

public class TwoEndedObject : MonoBehaviour
{
    public Transform player;
    public Transform farEnd;
    public float objectLength = 5f;

    private void Update()
    {
        // Get the mouse position in world space
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, player.position);
        float rayDistance;
        Vector3 targetPosition = transform.position;

        if (groundPlane.Raycast(mouseRay, out rayDistance))
        {
            Vector3 mousePosition = mouseRay.GetPoint(rayDistance);

            // Calculate the target position of the far end
            Vector3 playerToMouse = mousePosition - player.position;
            //playerToMouse.y = 0f;
            playerToMouse.Normalize();
            targetPosition = player.position + playerToMouse * objectLength;
        }

        // Update the object's positions
        transform.position = (player.position + targetPosition) * 0.5f;
        farEnd.position = targetPosition;
    }
}