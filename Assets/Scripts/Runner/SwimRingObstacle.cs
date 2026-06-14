using UnityEngine;

public class SwimRingObstacle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    private static readonly int ColorB = Shader.PropertyToID("_ColorB");
    private static readonly int Offset = Shader.PropertyToID("_Offset");
    private static readonly int StripeCount = Shader.PropertyToID("_StripeCount");

    private void Awake()
    {
        float stripeCount = meshRenderer.sharedMaterial.GetFloat(StripeCount);
        MaterialPropertyBlock mpb = new();
        meshRenderer.GetPropertyBlock(mpb);
        mpb.SetColor(ColorB, Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f));
        mpb.SetFloat(Offset, Random.Range(0f, 1f / stripeCount));
        meshRenderer.SetPropertyBlock(mpb);
    }
}
