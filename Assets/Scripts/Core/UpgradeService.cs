using UnityEngine;

public class UpgradeService : MonoBehaviour
{
    [SerializeField] private UpgradeConfig config;

    private void Awake() => UpgradeManager.Initialize(config);
}
