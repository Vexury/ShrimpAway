using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RunnerHUD : MonoBehaviour
{
    [SerializeField] private RollerController roller;
    [SerializeField] private TMP_Text speedLabel;
    [SerializeField] private string format = "{0:0} km/h";
    [SerializeField] private Image doubleJumpIcon;
    [SerializeField] private Color iconReadyColor = Color.white;
    [SerializeField] private Color iconGreyedColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    private void Update()
    {
        speedLabel.text = string.Format(format, roller.ForwardSpeed * 3.6f);
        if (doubleJumpIcon != null)
            doubleJumpIcon.color = roller.HasDoubleJump ? iconReadyColor : iconGreyedColor;
    }
}
