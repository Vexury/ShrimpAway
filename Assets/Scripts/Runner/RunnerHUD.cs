using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RunnerHUD : MonoBehaviour
{
    [SerializeField] private TrackManager trackManager;
    [SerializeField] private TMP_Text distanceLabel;

    [SerializeField] private RollerController roller;
    [SerializeField] private Image doubleJumpIcon;
    [SerializeField] private Image magnetIcon;
    [SerializeField] private Image magnetIconFill;
    [SerializeField] private MagnetEffect magnetEffect;
    [SerializeField] private Color iconReadyColor = Color.white;
    [SerializeField] private Color iconGreyedColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    private int _lastDistance = -1;

    private void Start()
    {
        if (magnetIcon != null)
        {
            magnetIcon.color = iconGreyedColor;
            magnetIcon.fillAmount = 1f;
        }
        if (magnetIconFill != null)
            magnetIconFill.fillAmount = 0f;
    }

    private void Update()
    {
        if (trackManager != null && distanceLabel != null)
        {
            int dist = (int)trackManager.DistanceTravelled;
            if (dist != _lastDistance)
            {
                _lastDistance = dist;
                distanceLabel.SetText("{0} m", (float)dist);
            }
        }

        if (doubleJumpIcon != null && roller != null)
            doubleJumpIcon.color = roller.HasDoubleJump ? iconReadyColor : iconGreyedColor;

        if (magnetIconFill != null && magnetEffect != null)
        {
            magnetIconFill.fillAmount = magnetEffect.IsActive && magnetEffect.TotalDuration > 0f
                ? magnetEffect.RemainingTime / magnetEffect.TotalDuration
                : 0f;
        }
    }
}
