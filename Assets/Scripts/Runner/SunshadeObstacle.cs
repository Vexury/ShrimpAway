using UnityEngine;

public class SunshadeObstacle : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Texture2D[] textures;

    private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

    private void Awake()
    {
        MaterialPropertyBlock mpb = new();
        meshRenderer.GetPropertyBlock(mpb);

        if (textures != null && textures.Length > 0)
            mpb.SetTexture(BaseMap, textures[Random.Range(0, textures.Length)]);

        if (Random.value < 0.5f)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }

        meshRenderer.SetPropertyBlock(mpb);
    }
}
