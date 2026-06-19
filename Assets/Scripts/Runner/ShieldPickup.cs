using UnityEngine;

public class ShieldPickup : PickupBase
{
    [SerializeField] private Renderer[] armorRenderers;

    protected override void Start()
    {
        base.Start();
        if (PlayerConfig.ArmorColor.HasValue)
            foreach (var r in armorRenderers) r.material.color = PlayerConfig.ArmorColor.Value;
    }

    protected override bool TryPickup(Collider other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health == null) return false;
        int amount = UpgradeManager.LastResortLevel > 0 && health.CurrentHP == 1 ? 2 : 1;
        health.Heal(amount);
        return true;
    }
}
