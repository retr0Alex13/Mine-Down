using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartLevel()
    {
        GameManager.Instance.ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
