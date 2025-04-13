using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class VRModelInteraction : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas infoCanvas;
    [SerializeField] private float uiDisplayDistance = 1.5f;
    [SerializeField] private float uiFollowSpeed = 5f;

    private Transform xrCamera;
    private bool isUIVisible = false;
    private XRSimpleInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        infoCanvas.gameObject.SetActive(false);

        // Set up interaction events
        interactable.selectEntered.AddListener(ShowInfoUI);
        //interactable.selectExited.AddListener(HideInfoUI);
        Debug.Log("Awakened Without issue.");
    }

    private void Start()
    {
        xrCamera = Camera.main.transform;

        // If using XR Origin
        if (xrCamera == null && GameObject.Find("XR Origin") != null)
        {
            xrCamera = GameObject.Find("XR Origin (XR Rig)").transform.Find("Camera Offset");
        }
    }

    private void Update()
    {
        //if (isUIVisible)
        //{
        //    // Position UI in front of player
        //    Vector3 targetPosition = xrCamera.position + xrCamera.forward * uiDisplayDistance;
        //    infoCanvas.transform.position = Vector3.Lerp(
        //        infoCanvas.transform.position,
        //        targetPosition,
        //        uiFollowSpeed * Time.deltaTime);

        //    // Make UI face player
        //    infoCanvas.transform.LookAt(xrCamera);
        //    infoCanvas.transform.rotation *= Quaternion.Euler(0, 180, 0);
        //}
    }

    private void ShowInfoUI(SelectEnterEventArgs args)
    {
        isUIVisible = true;
        infoCanvas.gameObject.SetActive(true);
        Debug.Log("UI is now visible.");
    }

    private void HideInfoUI(SelectExitEventArgs args)
    {
        isUIVisible = false;
        infoCanvas.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        interactable.selectEntered.RemoveListener(ShowInfoUI);
        interactable.selectExited.RemoveListener(HideInfoUI);
    }
}