using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DartGameManager : MonoBehaviour
{
    [SerializeField] private XRInteractionManager interactionManager;
    [SerializeField] private GameObject dartPrefab;
    [SerializeField] private Transform dartSpawnPoint;
    [SerializeField] private int maxDarts = 3;

    private Vector3 dartSpawnPosition;
    private Quaternion dartSpawnRotation; // Adjust as needed

    private int currentDarts = 0;
    private int totalScore = 0;

    private void Start()
    {
        dartSpawnPosition = dartSpawnPoint.position;
        dartSpawnRotation = dartSpawnPoint.rotation;
    }

    public void SpawnDart()
    {
        if (currentDarts >= maxDarts) return;

        GameObject newDart = Instantiate(dartPrefab, dartSpawnPosition, dartSpawnRotation);
        XRGrabInteractable grabInteractable = newDart.GetComponent<XRGrabInteractable>();

        currentDarts++;
    }

    public void DartLanded(int score)
    {
        totalScore += score;
        Debug.Log($"Total Score: {totalScore}");

        // Spawn a new dart after a short delay
        Invoke(nameof(SpawnDart), 1f);
    }

    public void ResetGame()
    {
        totalScore = 0;
        currentDarts = 0;

        // Destroy all existing darts
        Dart[] existingDarts = FindObjectsByType<Dart>(FindObjectsSortMode.None);
        foreach (Dart dart in existingDarts)
        {
            Destroy(dart.gameObject);
        }

        SpawnDart();
    }
}