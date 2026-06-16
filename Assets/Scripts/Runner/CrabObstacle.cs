using UnityEngine;

public class CrabObstacle : MonoBehaviour
{
    [SerializeField] private Flipbook flipbook;
    [SerializeField] private AudioClip sfxClip;
    [SerializeField] private float sfxMinDistance = 5f;
    [SerializeField] private float sfxMaxDistance = 20f;

    private void OnEnable()  => flipbook.OnLastFrame += PlaySound;
    private void OnDisable() => flipbook.OnLastFrame -= PlaySound;

    private void PlaySound()
    {
        if (sfxClip != null && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFXAtPoint(sfxClip, transform.position, sfxMinDistance, sfxMaxDistance);
    }
}
