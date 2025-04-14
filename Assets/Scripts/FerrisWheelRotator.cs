using UnityEngine;

public class FerrisWheelRotator : MonoBehaviour
{
    public float rotationSpeed = 5f;  // Speed of rotation in degrees per second

    private Transform wheel;
    private Transform[] chairs;
    private float currentAngle = 0f;

    void Start()
    {
        // Find the wheel GameObject under FerrisWheel
        wheel = transform.Find("Wheel");
        if (wheel == null)
        {
            Debug.LogError("Could not find 'Wheel' under " + name);
            return;
        }

        // Find the Chairs container under FerrisWheel
        Transform chairsParent = transform.Find("Chairs");
        if (chairsParent == null)
        {
            Debug.LogError("Could not find 'Chairs' under " + name);
            return;
        }

        // Get all chairs under the Chairs object
        chairs = new Transform[chairsParent.childCount];
        for (int i = 0; i < chairs.Length; i++)
        {
            chairs[i] = chairsParent.GetChild(i);
        }
    }

    void Update()
    {
        if (wheel == null || chairs == null || chairs.Length == 0) return;

        // Rotate the wheel (purely visual)
        wheel.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Update the current rotation angle
        currentAngle += rotationSpeed * Time.deltaTime;

        // Update the position of each chair
        for (int i = 0; i < chairs.Length; i++)
        {
            float angleOffset = 360f / chairs.Length * i;
            float angle = (currentAngle + angleOffset) * Mathf.Deg2Rad;

            // Calculate the radius from the center of the wheel
            float radius = Vector3.Distance(wheel.position, chairs[i].position);

            // Update chair's new position
            Vector3 newPos = wheel.position + new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );

            chairs[i].position = newPos;

            // Keep each chair upright
            chairs[i].rotation = Quaternion.identity;
        }
    }
}
