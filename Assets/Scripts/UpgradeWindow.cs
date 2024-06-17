using System.Collections.Generic;
using UnityEngine;

public class UpgradeWindow : MonoBehaviour
{
    // TO DO: Play the upgrade sound
    // TO DO: Add the upgrade to the player
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private GameObject itemsPanel;

    private List<GameObject> upgrades;

    private void Awake()
    {
        upgrades = new List<GameObject>();
        foreach (Transform child in itemsPanel.transform)
        {
            upgrades.Add(child.gameObject);

            if (child.TryGetComponent(out UpgradeItem item))
            {
                item.OnUpgradeBought += OnUpgradeBought;
            }
        }
    }

    private void Update()
    {
        HandleUpgradeButton();
    }

    private void OnDestroy()
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.TryGetComponent(out UpgradeItem item))
            {
                item.OnUpgradeBought -= OnUpgradeBought;
            }
        }
    }

    private void OnEnable()
    {
        HandleUpgradeButton();
    }

    public void HandleUpgradeButton()
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.TryGetComponent(out UpgradeItem item))
            {
                item.CheckAbilityAccess(scoreController.Score);
            }
        }
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }

    private void OnUpgradeBought(Upgrade upgrade)
    {
        scoreController.AddScore(-upgrade.Cost);
        HandleUpgradeButton();
    }

}
