using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    int blocksLeft;
    const int lastScene = 6;
    bool quit = false;
    int activeSceneIndex;

    [SerializeField] AudioClip quitGameClip;
    [SerializeField] AudioClip levelEndClip;
    [SerializeField] AudioClip cheersClip;
    [SerializeField] private AudioSource sfxAudioSource, musicAudioSource;

    public float timer = 0;
    public bool timerOn = false;
    public float minimunTime = 999999f;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
        private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        activeSceneIndex = scene.buildIndex;
        blocksLeft = GameObject.FindGameObjectsWithTag("Block").Length;
        // End game logic goes here
        if (activeSceneIndex > 0 && activeSceneIndex < lastScene)
        {
            timerOn = true;
        }
        if (activeSceneIndex == lastScene)
        {
            TextMeshProUGUI score = GameObject.Find("scoreText")?.GetComponent<TextMeshProUGUI>();
            timerOn = false;
            score.text = string.Format("{0:00000}", Math.Floor(timer));
            if (timer < minimunTime)
            {
                ActivateImageByName("Smiley");
                PlaySoundEffect(cheersClip, 0.5f);
                SaveScore((int) timer);
            }
        }
    }

    public void ActivateImageByName(string objectName)
    {
        var objs = FindObjectsByType<Image>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var img in objs)
        {
            if (img.name == objectName)
            {
                img.gameObject.SetActive(true);
            }
        }
    }

    void Start()
    {
        ReadScore();
        // ResetScore();
        // SceneManager.LoadScene(5);
        // timerOn = true;
    }

    
    public void RestartScene()
    {
        SceneManager.LoadScene(activeSceneIndex);
    }

    public void RestartGame()
    {
        activeSceneIndex = 0; // StartScreen
        SceneManager.LoadScene(activeSceneIndex);
        timerOn = false;

    }

    public void DecreaseBlock()
    {
        blocksLeft--;
        if (blocksLeft == 0)
        {
            PlaySoundEffect(levelEndClip, 0.5f);
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        int nextSceneIndex = activeSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void Update()
    {
        if (timerOn == true)
        {
            timer += Time.deltaTime;
        }
        if (Input.GetKey("escape") && activeSceneIndex > 0 && activeSceneIndex < lastScene)
        {
            Debug.Log("Escape key pressed. Attempting to quit the game.");
            if (quit == false)
            {
                quit = true;
                QuitGame();
            }
        }
    }

    public void QuitGame()
    {
        BallMovement.StopBall();
        StartCoroutine(QuitAfterSound());
    }

    private IEnumerator QuitAfterSound()
    {
        PlaySoundEffect(quitGameClip, 0.5f);
        yield return new WaitForSeconds(quitGameClip.length);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void ReadScore()
    {
        if (PlayerPrefs.HasKey("highScore"))
        {
            minimunTime = PlayerPrefs.GetInt("highScore");
            if (minimunTime == 0)
            {
                Debug.Log("No high score found, setting to default value.");
                PlayerPrefs.SetInt("highScore", 999999);
                PlayerPrefs.Save();
                minimunTime = 999999f;
            }
        }
        else
        {
            minimunTime = 999999f;
            PlayerPrefs.SetInt("highScore", (int)minimunTime);
            PlayerPrefs.Save();
        }
    }
    public void SaveScore(int minimun)
    {
        PlayerPrefs.SetInt("highScore", minimun);
        PlayerPrefs.Save();
    }

    public void ResetScore()
    {
        minimunTime = 999999f;
        PlayerPrefs.SetInt("highScore", (int) minimunTime);
        PlayerPrefs.Save();
    }

    public void PlaySoundEffect(AudioClip audioClip, float volume)
    {
        sfxAudioSource.PlayOneShot(audioClip, volume);
    }

}
