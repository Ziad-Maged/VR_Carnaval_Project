using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
public class Dart : MonoBehaviour
{
    [SerializeField] private float throwForceMultiplier = 1.5f;
    [SerializeField] private float dartStickForce = 10f;
    [SerializeField] private float dartStickTorque = 5f;
    [SerializeField] private Dartboard dartboard;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private bool hasBeenThrown = false;
    private Vector3 previousPosition;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grabInteractable.selectExited.AddListener(OnRelease);

        if(dartboard == null)
        {
            dartboard = GameObject.FindGameObjectWithTag("Dartboard").GetComponent<Dartboard>();
            if (dartboard == null)
            {
                Debug.LogError("Dartboard not found in the scene.");
            }
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        //if (args.interactorObject is XRDirectInteractor)
        //{
        //    hasBeenThrown = true;
        //    Vector3 velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
        //    rb.AddForce(velocity * throwForceMultiplier, ForceMode.Impulse);
        //    Debug.Log("Dart thrown with velocity: " + velocity);
        //}

        hasBeenThrown = true;
        Vector3 velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
        velocity.z = -2.0f;
        velocity.y = 2.0f;
        rb.AddForce(velocity * throwForceMultiplier, ForceMode.Impulse);
        Debug.Log("Dart thrown with velocity: " + velocity);
    }

    private void FixedUpdate()
    {
        previousPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasBeenThrown) return;

        if (collision.gameObject.CompareTag("Dartboard"))
        {
            StickDart(collision);
            ScoreDart(collision.contacts[0].point);
            GetComponent<AudioSource>().Play();
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            // Prevent darts from bouncing off walls
            Debug.Log("Dart hit a wall.");
            dartboard.HitWall();
            rb.isKinematic = true;
            grabInteractable.enabled = false;
        }
    }

    private void StickDart(Collision collision)
    {
        // Make the dart stick to the surface
        rb.isKinematic = true;

        // Attach to the dartboard (optional)
        transform.SetParent(collision.transform);

        // Apply slight force to make it stick better
        rb.AddForce(-collision.contacts[0].normal * dartStickForce, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * dartStickTorque, ForceMode.Impulse);

        grabInteractable.enabled = false;
    }

    private void ScoreDart(Vector3 hitPoint)
    {
        // You'll need to implement your scoring logic here
        dartboard.CalculateScore(hitPoint);
        // This would involve determining which section of the dartboard was hit
        Debug.Log("Dart hit at: " + hitPoint);

        // Disable interaction after sticking
        grabInteractable.enabled = false;
    }

    private void OnDestroy()
    {
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }
}