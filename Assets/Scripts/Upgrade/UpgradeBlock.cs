using UnityEngine;

public class UpgradeBlock : MonoBehaviour
{
    private UpgradeWindow upgradeWindow;

    private void Start()
    {
        upgradeWindow = (UpgradeWindow)FindObjectOfType(typeof(UpgradeWindow), true);
        Debug.Log("Upgrade Window Componenet" + upgradeWindow);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMove player))
        {
            SoundManager.instance.Play("Pop");

            upgradeWindow.gameObject.SetActive(true);
            upgradeWindow.HandleUpgradeButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerMove player))
        {
            upgradeWindow.gameObject.SetActive(false);
        }
    }
}
