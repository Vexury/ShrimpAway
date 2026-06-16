using UnityEngine;

public class HoverFloat : MonoBehaviour
{
    [SerializeField] float maxAmplitude = 0.3f;
    [SerializeField] TrackManager trackManager;

    Vector3 _startLocalPos;

    void Start()
    {
        _startLocalPos = transform.localPosition;
    }

    void Update()
    {
        if (trackManager == null) return;

        float t = Mathf.InverseLerp(trackManager.InitialSpeed, trackManager.MaxSpeed, trackManager.WorldSpeed);
        transform.localPosition = _startLocalPos + transform.localRotation * Vector3.up * (maxAmplitude * t);
    }
}
