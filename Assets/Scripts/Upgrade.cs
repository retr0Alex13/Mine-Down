using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Bson;
using UnityEngine;

public enum UpgradeType
{
    Range,
    Strength,
}

public class Upgrade : MonoBehaviour
{
    [field: SerializeField] 
    public string Name { get; private set; }

    [field: SerializeField]
    public int Cost { get; private set; }

    [field: SerializeField]
    public bool IsPurchased { get; private set; } = false;

    [field: SerializeField]
    public UpgradeType upgradeType { get; private set; }

    public void SetPurchased(bool value)
    {
        IsPurchased = value;
    }
}
