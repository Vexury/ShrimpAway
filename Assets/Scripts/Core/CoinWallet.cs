using System;

public static class CoinWallet
{
    public static int Total { get; private set; }
    public static event Action<int> OnTotalChanged;

    public static void Add(int amount)
    {
        Total += amount;
        OnTotalChanged?.Invoke(Total);
    }

    public static bool TrySpend(int amount)
    {
        if (Total < amount) return false;
        Total -= amount;
        return true;
    }
}
