using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ColorSwatch : MonoBehaviour
{
    [SerializeField] private Color color = Color.white;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => PlayerConfig.SetColor(color));
    }
}
