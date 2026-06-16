using System;
using UnityEngine;

public enum UpgradeType { HeadStart, HPBoost, CoinBooster, ExtraLife }

public static class UpgradeManager
{
    public static int HeadStartLevel   { get; private set; }
    public static int HPBoostLevel     { get; private set; }
    public static int CoinBoosterLevel { get; private set; }
    public static int ExtraLifeLevel   { get; private set; }
    public static bool ExtraLifeUsedThisRun { get; private set; }
    public static bool HPBoostUsedThisRun   { get; private set; }

    public static float RedCoinChance => CoinBoosterLevel * 0.1f;
    public static int HeadStartDistance => 500 * HeadStartLevel;

    private static UpgradeConfig _config;

    public static void Initialize(UpgradeConfig config) => _config = config;

    public static int GetLevel(UpgradeType type) => type switch
    {
        UpgradeType.HeadStart   => HeadStartLevel,
        UpgradeType.HPBoost     => HPBoostLevel,
        UpgradeType.CoinBooster => CoinBoosterLevel,
        UpgradeType.ExtraLife   => ExtraLifeLevel,
        _                       => 0
    };

    public static int NextCost(UpgradeType type)
    {
        int[] costs = GetCosts(type);
        int level   = GetLevel(type);
        return level < costs.Length ? costs[level] : -1;
    }

    public static bool CanPurchase(UpgradeType type)
    {
        int cost = NextCost(type);
        return cost >= 0 && CoinWallet.Total >= cost;
    }

    public static bool Purchase(UpgradeType type)
    {
        int cost = NextCost(type);
        if (cost < 0 || !CoinWallet.TrySpend(cost)) return false;
        switch (type)
        {
            case UpgradeType.HeadStart:   HeadStartLevel++;   break;
            case UpgradeType.HPBoost:     HPBoostLevel++;     break;
            case UpgradeType.CoinBooster: CoinBoosterLevel++; break;
            case UpgradeType.ExtraLife:   ExtraLifeLevel++;   break;
        }
        return true;
    }

    public static void UseExtraLife()  => ExtraLifeUsedThisRun = true;
    public static void UseHPBoost()    { HPBoostUsedThisRun = true; HPBoostLevel = 0; }
    public static void ResetRunState() { ExtraLifeUsedThisRun = false; HPBoostUsedThisRun = false; }

    private static int[] GetCosts(UpgradeType type) => type switch
    {
        UpgradeType.HeadStart   => _config.headStartCosts,
        UpgradeType.HPBoost     => _config.hpBoostCosts,
        UpgradeType.CoinBooster => _config.coinBoosterCosts,
        UpgradeType.ExtraLife   => _config.extraLifeCosts,
        _                       => Array.Empty<int>()
    };
}
