using UnityEngine;

public class ShieldPickup : PickupBase
{
    protected override bool TryPickup(Collider other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health == null) return false;
        health.Heal(1);
        return true;
    }
}
