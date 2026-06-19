using UnityEngine;

public class CharacterPreview : MonoBehaviour
{
    [SerializeField] private Renderer[] bodyRenderers;
    [SerializeField] private Renderer[] armorRenderers;
    [SerializeField] private Transform geometry;
    [SerializeField] private Transform yRotationTarget;
    [SerializeField] private bool rotateX = true;
    [SerializeField] private float rotationSpeedX = 45f;
    [SerializeField] private bool rotateY = false;
    [SerializeField] private float rotationSpeedY = 45f;

    private void OnEnable()  => PlayerConfig.OnColorChanged += OnColorChanged;
    private void OnDisable() => PlayerConfig.OnColorChanged -= OnColorChanged;

    private void Start()
    {
        if (PlayerConfig.BodyColor.HasValue)  ApplyColor(PlayerConfig.ColorTarget.Body,  PlayerConfig.BodyColor.Value);
        if (PlayerConfig.ArmorColor.HasValue) ApplyColor(PlayerConfig.ColorTarget.Armor, PlayerConfig.ArmorColor.Value);
    }

    public bool Paused { get; set; }

    private void Update()
    {
        if (Paused) return;
        if (rotateX) geometry.Rotate(Vector3.right, rotationSpeedX * Time.deltaTime, Space.Self);
        if (rotateY)
        {
            float yDelta = rotationSpeedY * Time.deltaTime;
            geometry.Rotate(Vector3.up, yDelta, Space.World);
            if (yRotationTarget != null) yRotationTarget.Rotate(Vector3.up, yDelta, Space.World);
        }
    }

    private void OnColorChanged(PlayerConfig.ColorTarget target, Color color) => ApplyColor(target, color);

    private void ApplyColor(PlayerConfig.ColorTarget target, Color color)
    {
        Renderer[] renderers = target == PlayerConfig.ColorTarget.Body ? bodyRenderers : armorRenderers;
        foreach (var r in renderers) r.material.color = color;
    }
}
