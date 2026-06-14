using UnityEngine;
using UnityEngine.VFX;

public class SpeedVFXDriver : MonoBehaviour
{
    [SerializeField] private VisualEffect vfx;
    [SerializeField] private TrackManager trackManager;

    private static readonly int SparksRate = Shader.PropertyToID("SparksRate");

    private void Update()
    {
        vfx.SetFloat(SparksRate, trackManager.WorldSpeed * 4.0f);
    }
}
