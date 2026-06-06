using TMPro;
using UnityEngine;

public class ControllerSwapper : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TopDownController topDownController;
    [SerializeField] private VehicleControllerRigidbody vehicleController;
    [SerializeField] private GameObject topDownGeometry;
    [SerializeField] private GameObject carGeometry;
    [SerializeField] private TMP_Text controllerLabel;

    [Header("Labels")]
    [SerializeField] private string topDownLabel = "Top Down";
    [SerializeField] private string vehicleLabel = "Vehicle";

    [Header("TopDown Capsule")]
    [SerializeField] private float topDownHeight = 2f;
    [SerializeField] private float topDownRadius = 0.4f;
    [SerializeField] private Vector3 topDownCenter = new Vector3(0f, 1f, 0f);

    public enum ControllerMode { TopDown, Vehicle }

    [Header("Default")]
    [SerializeField] private ControllerMode defaultMode = ControllerMode.TopDown;

    private ControllerMode currentMode;

    private void Start()
    {
        currentMode = defaultMode;
        Apply();
    }

    private void OnEnable()
    {
        inputReader.NextEvent += Cycle;
    }

    private void OnDisable()
    {
        inputReader.NextEvent -= Cycle;
    }

    private void Cycle()
    {
        currentMode = currentMode == ControllerMode.TopDown ? ControllerMode.Vehicle : ControllerMode.TopDown;
        Apply();
    }

    private void Apply()
    {
        bool isTopDown = currentMode == ControllerMode.TopDown;

        topDownController.enabled = isTopDown;
        vehicleController.enabled = !isTopDown;
        topDownGeometry.SetActive(isTopDown);
        carGeometry.SetActive(!isTopDown);

        characterController.enabled = isTopDown;
        rb.isKinematic = isTopDown;

        if (isTopDown)
        {
            characterController.height = topDownHeight;
            characterController.radius = topDownRadius;
            characterController.center = topDownCenter;
        }

        if (controllerLabel != null)
            controllerLabel.text = isTopDown ? topDownLabel : vehicleLabel;
    }
}
