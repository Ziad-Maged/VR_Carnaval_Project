using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Dartboard : MonoBehaviour
{
    [SerializeField] private Transform bullseyeCenter;
    [SerializeField] private float bullseyeRadius = 0.05f;
    [SerializeField] private float outerBullRadius = 0.1f;
    [SerializeField] private float tripleRingRadius = 0.3f;
    [SerializeField] private float doubleRingRadius = 0.4f;
    [SerializeField] private float maxRadius = 0.5f;
    [SerializeField] private DartGameManager gameManager;

    public void CalculateScore(Vector3 hitPoint)
    {
        // Convert hit point to local space of dartboard
        Vector3 localHit = bullseyeCenter.InverseTransformPoint(hitPoint);
        localHit.z = 0; // Ignore depth

        float distance = localHit.magnitude;
        int score = 0;
        string scoreType = "";

        if (distance < bullseyeRadius)
        {
            score = 50;
            scoreType = "Bullseye!";
        }
        else if (distance < outerBullRadius)
        {
            score = 25;
            scoreType = "Outer Bull";
        }
        else if (distance < maxRadius)
        {
            // Calculate angle to determine sector (1-20)
            float angle = Mathf.Atan2(localHit.y, localHit.x) * Mathf.Rad2Deg;
            angle = (angle + 360 + 9) % 360; // Offset by 9 degrees to center sectors

            int sector = Mathf.FloorToInt(angle / 18f) + 1;
            int[] sectorValues = { 20, 1, 18, 4, 13, 6, 10, 15, 2, 17, 3, 19, 7, 16, 8, 11, 14, 9, 12, 5 };
            score = sectorValues[sector % 20];

            // Check for double or triple rings
            if (distance > doubleRingRadius && distance < doubleRingRadius + 0.02f)
            {
                score *= 2;
                scoreType = "Double " + score / 2;
            }
            else if (distance > tripleRingRadius && distance < tripleRingRadius + 0.02f)
            {
                score *= 3;
                scoreType = "Triple " + score / 3;
            }
            else
            {
                scoreType = "Single " + score;
            }
        }

        // Update the game manager with the score
        gameManager.DartLanded(score);
        Debug.Log($"Score: {score} ({scoreType})");
        // Here you would update your score UI
    }

    public void HitWall()
    {
        gameManager.DartLanded(0);
    }
}