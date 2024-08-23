using System.Collections.Generic;
using UnityEngine;

public class UpgradeWindow : MonoBehaviour
{
    [SerializeField] private ScoreController scoreController;
    [SerializeField] private PlayerAttack playerAttack;
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
        SoundManager.instance.Play("Pop");
        gameObject.SetActive(false);
    }

    private void OnUpgradeBought(Upgrade upgrade)
    {
        SoundManager.instance.Play("Upgrade");
        scoreController.AddScore(-upgrade.Cost);
        playerAttack.SetUpgradeSlot(upgrade.upgradeType);
    }

}
