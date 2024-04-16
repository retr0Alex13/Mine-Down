using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private ScoreController scoreController;
    public static bool IsGamePaused { get; private set; }
    public int HighScore { get; private set; }

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PauseGame()
    {
        IsGamePaused = true;
        AudioListener.pause = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        IsGamePaused = false;
        AudioListener.pause = false;
        Time.timeScale = 1f;
    }

    public void RestartLevel()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //private void ProcessRestartLevel()
    //{
    //    StartCoroutine(RestartLevelWithDelay());
    //}

    //public IEnumerator RestartLevelWithDelay()
    //{
    //    yield return new WaitForSeconds(RestartDelay);
    //    RestartLevel();
    //}

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void ToggleSound()
    {
        float currentVolume = 0f;
        audioMixer.GetFloat("volume", out currentVolume);

        if (currentVolume == 0f)
        {
            SetVolume(-80f);
        }
        else
        {
            SetVolume(0f);
        }
    }

    public void SetHighScoreAsPlayers()
    {
        HighScore = scoreController.Score;
    }
}
