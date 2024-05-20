using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private ScoreController scoreController;
    public static bool IsGamePaused { get; private set; }
    public int HighScore { get; private set; }
    public bool IsMuted { get; private set; }

    public static GameManager Instance;

    private const float RestartDelay = 3f;

    private int gamePlayed = 1;

    private Coroutine restartLevelCoroutine;

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

        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += CancelLevelRestart;
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            scoreController = FindObjectOfType<ScoreController>();
        };

        LoadHighScore();
        Application.targetFrameRate = 30;
    }

    private void LoadHighScore()
    {
        if (PlayerPrefs.GetInt("HighScore") > 0)
        {
            HighScore = PlayerPrefs.GetInt("HighScore");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= CancelLevelRestart;
    }

    public void CancelLevelRestart(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (restartLevelCoroutine == null)
        {
            return;
        }
        StopCoroutine(restartLevelCoroutine);
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

    public void ProcessRestartLevel()
    {
        restartLevelCoroutine = StartCoroutine(RestartLevelWithDelay());
    }

    private IEnumerator RestartLevelWithDelay()
    {
        yield return new WaitForSeconds(RestartDelay);
        RestartLevel();
    }

    public void RestartLevel()
    {
        ResumeGame();
        gamePlayed++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (gamePlayed % 3 == 0)
        {
            AdsManager.Instance.interstitialAd.ShowAd();
        }
    }

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
            IsMuted = true;
        }
        else
        {
            SetVolume(0f);
            IsMuted = false;
        }
    }

    public void SetHighScoreAsPlayers()
    {
        HighScore = scoreController.Score;
        PlayerPrefs.SetInt("HighScore", HighScore);
    }
}
