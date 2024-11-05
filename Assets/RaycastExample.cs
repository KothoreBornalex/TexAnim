using UnityEngine;

public class RaycastExample : MonoBehaviour
{
    public float angle;
    public float maxDistance = 10f;
    public LayerMask layerMask;

    void Update()
    {
        /*// Calculate the starting angle
        float startAngle = -35f;
        // Calculate the ending angle
        float endAngle = 35f;
        // Calculate the angle increment
        float angleIncrement = 10f;

        // Iterate through angles
        for (float angle = startAngle; angle <= endAngle; angle += angleIncrement)
        {
            // Calculate direction based on the angle
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * transform.right;

            // Perform the raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, layerMask);

            // Check if the raycast hit something
            if (hit.collider != null)
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
            }
            else
            {
                // Draw debug line for raycast
                Vector3 endPosition = transform.position + direction * maxDistance;
                Debug.DrawLine(transform.position, endPosition, Color.green);
            }
        }*/



        Vector3 direction = Quaternion.Euler(0f, 0f, angle) * transform.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, layerMask);

        Vector3 endPosition = transform.position + direction * maxDistance;
        Debug.DrawLine(transform.position, endPosition, Color.green);
    }
}