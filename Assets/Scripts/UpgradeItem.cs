using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    public event Action<Upgrade> OnUpgradeBought;

    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI upgradeCost;

    [SerializeField] private Button buyUpgradeButton;
    
    private Upgrade upgrade;

    private void Awake()
    {
        Upgrade upgradeComponent = GetComponent<Upgrade>();
        if (upgradeComponent != null)
        {
            upgrade = upgradeComponent;
        }
        else
        {
            Debug.LogError("Upgrade component not found on GameObject: " + gameObject.name);
        }
    }

    private void Start()
    {
        SetItem();
    }

    private void Update()
    {
        Debug.Log(upgrade.IsPurchased);
    }

    public void CheckAbilityAccess(int score)
    {
        if (upgrade.IsPurchased)
        {
            buyUpgradeButton.interactable = false;
        }
        else if (score >= upgrade.Cost)
        {
            buyUpgradeButton.interactable = true;
        }
        else
        {
            buyUpgradeButton.interactable = false;
        }
    }

    private void SetItem()
    {
        upgradeName.text = upgrade.Name;
        upgradeCost.text = upgrade.Cost.ToString();
    }

    public void BuyUpgrade()
    {
        OnUpgradeBought?.Invoke(upgrade);
        upgrade.SetPurchased(true);
    }
}
