using UnityEngine;

public class ColorTargetSelector : MonoBehaviour
{
    public void SelectBody()  => PlayerConfig.ActiveTarget = PlayerConfig.ColorTarget.Body;
    public void SelectArmor() => PlayerConfig.ActiveTarget = PlayerConfig.ColorTarget.Armor;
}
