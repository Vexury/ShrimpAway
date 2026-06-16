using UnityEngine;

public class CharacterPreview : MonoBehaviour
{
    [SerializeField] private Renderer[] bodyRenderers;
    [SerializeField] private Renderer[] armorRenderers;
    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private Transform geometry;

    private void OnEnable()  => PlayerConfig.OnColorChanged += OnColorChanged;
    private void OnDisable() => PlayerConfig.OnColorChanged -= OnColorChanged;

    private void Start()
    {
        if (PlayerConfig.BodyColor.HasValue)  ApplyColor(PlayerConfig.ColorTarget.Body,  PlayerConfig.BodyColor.Value);
        if (PlayerConfig.ArmorColor.HasValue) ApplyColor(PlayerConfig.ColorTarget.Armor, PlayerConfig.ArmorColor.Value);
    }

    private void Update()
    {
        geometry.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }

    private void OnColorChanged(PlayerConfig.ColorTarget target, Color color) => ApplyColor(target, color);

    private void ApplyColor(PlayerConfig.ColorTarget target, Color color)
    {
        Renderer[] renderers = target == PlayerConfig.ColorTarget.Body ? bodyRenderers : armorRenderers;
        foreach (var r in renderers) r.material.color = color;
    }
}
