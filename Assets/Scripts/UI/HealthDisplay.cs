using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image[] slotImages;
    [SerializeField] private Sprite[] filledSprites;
    [SerializeField] private Sprite[] outlineSprites;
    [SerializeField] private float blinkInterval = 0.35f;
    [SerializeField] private int blinkCount = 3;

    private CanvasGroup _canvasGroup;
    private Coroutine _blinkCoroutine;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup != null) _canvasGroup.alpha = 0f;
    }

    private void OnEnable()
    {
        PlayerHealth.OnDamaged += HandleChange;
        PlayerHealth.OnHealed  += HandleChange;
    }

    private void OnDisable()
    {
        PlayerHealth.OnDamaged -= HandleChange;
        PlayerHealth.OnHealed  -= HandleChange;
    }

    private void Start()
    {
        if (PlayerConfig.ArmorColor.HasValue)
            foreach (var img in slotImages)
                if (img != null) img.color = PlayerConfig.ArmorColor.Value;

        if (playerHealth == null) return;
        Refresh(playerHealth.CurrentHP, playerHealth.MaxHP);
        Trigger();
    }

    private void HandleChange(int hp, int maxHP)
    {
        Refresh(hp, maxHP);
        Trigger();
    }

    private void Refresh(int hp, int maxHP)
    {
        int armorCount  = maxHP - 1;
        int filledCount = hp - 1;
        for (int i = 0; i < slotImages.Length; i++)
        {
            if (slotImages[i] == null) continue;
            bool inRange = i < armorCount;
            slotImages[i].gameObject.SetActive(inRange);
            if (inRange)
                slotImages[i].sprite = i < filledCount ? filledSprites[i] : outlineSprites[i];
        }
    }

    private void Trigger()
    {
        if (_blinkCoroutine != null) StopCoroutine(_blinkCoroutine);
        _blinkCoroutine = StartCoroutine(BlinkAndHide());
    }

    private IEnumerator BlinkAndHide()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            SetVisible(true);
            yield return new WaitForSeconds(blinkInterval);
            SetVisible(false);
            yield return new WaitForSeconds(blinkInterval);
        }
        _blinkCoroutine = null;
    }

    private void SetVisible(bool visible)
    {
        if (_canvasGroup != null)
            _canvasGroup.alpha = visible ? 1f : 0f;
    }
}
