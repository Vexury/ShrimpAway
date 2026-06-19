using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterPreviewInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CharacterPreview preview;

    public void OnPointerDown(PointerEventData _) => preview.Paused = true;
    public void OnPointerUp(PointerEventData _)   => preview.Paused = false;
}
