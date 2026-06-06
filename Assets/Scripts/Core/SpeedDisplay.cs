using TMPro;
using UnityEngine;

public class SpeedDisplay : MonoBehaviour
{
    [SerializeField] private MonoBehaviour vehicleSource;
    [SerializeField] private TMP_Text label;
    [SerializeField] private string format = "{0:0} km/h";

    private IVehicle vehicle;

    private void Awake()
    {
        vehicle = vehicleSource as IVehicle;
    }

    private void Update()
    {
        if (vehicle == null) return;
        label.text = string.Format(format, Mathf.Abs(vehicle.CurrentSpeed) * 3.6f)
            + $"\nGrounded: {vehicle.IsGrounded}";
    }
}
