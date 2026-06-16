public static class CoinWallet
{
    public static int Total { get; private set; }

    public static void Add(int amount) => Total += amount;

    public static bool TrySpend(int amount)
    {
        if (Total < amount) return false;
        Total -= amount;
        return true;
    }
}
