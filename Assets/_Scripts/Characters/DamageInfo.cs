using UnityEngine;

public readonly struct DamageInfo
{
    public readonly GameObject Source;
    public readonly float Amount;
    public readonly bool AffectsBigResource;
    public DamageInfo(GameObject source, float amount, bool affectsBigResource)
    {
        this.Source = source;
        this.Amount = amount;
        this.AffectsBigResource = affectsBigResource;
    }
}