using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeConfig", menuName = "Game/Upgrade Config")]
public class UpgradeConfig : ScriptableObject
{
    public int[] headStartCosts   = { 2000 };
    public int[] hpBoostCosts     = { 3000 };
    public int[] coinBoosterCosts = { 1500 };
    public int[] extraLifeCosts   = { 5000 };
}
