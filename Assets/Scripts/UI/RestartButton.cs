using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartLevel()
    {
        GameManager.Instance.ResumeGame();
        GameManager.Instance.RestartLevel();
    }
}
