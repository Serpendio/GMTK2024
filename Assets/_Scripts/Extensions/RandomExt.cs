
using UnityEngine;

public static class RandomExt
{
    /// <summary>
    /// retuns 1 or -1
    /// </summary>
    public static int Sign()
        => (Random.Range(0, 2) * 2) - 1;
}

