using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.InputSystem;

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
    [SerializeField] AudioSource sfxAudioSource, musicAudioSource;

    TextMeshProUGUI scoreText;
    TextMeshProUGUI minimunText;

    BallMovement ballMovement;

    public float timer = 0;
    public bool timerOn = false;
    public float minimunTime = 999999f;

    private PlayerInput playerInput;

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
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.Log("PlayerInput component not found on GameManager.");
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
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
            minimunText = GameObject.Find("MinimunText")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI level = GameObject.Find("LevelText")?.GetComponent<TextMeshProUGUI>();
            level.text = string.Format("{00000}", activeSceneIndex);
            ballMovement = FindFirstObjectByType<BallMovement>();
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
                SaveScore((int)timer);
            }
        }
    }

    public void UpdateScore()
    {
        scoreText.text = string.Format(" {00000}", Math.Floor(timer));
        minimunText.text = string.Format(" {00000}", Math.Floor(minimunTime));
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
        //ResetScore();
        //SceneManager.LoadScene(5); // For testing purposes, load the end scene directly
        //timerOn = true;
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
        bool exit = playerInput.actions["Exit"].WasPressedThisFrame();
        bool changeFullscreen = playerInput.actions["ChangeFullscreen"].WasPressedThisFrame();
        bool tilt = playerInput.actions["Tilt"].WasPressedThisFrame();

        if (timerOn == true)
        {
            timer += Time.deltaTime;
        }
        if (exit && activeSceneIndex > 0 && activeSceneIndex < lastScene)
        {
            if (quit == false)
            {
                quit = true;
                QuitGame();
            }
        }
        if (tilt && activeSceneIndex > 0 && activeSceneIndex < lastScene)
        {
            if (quit == false)
            {
                ballMovement.Tilt();
            }
        }

        if (changeFullscreen)
        {
            if (quit == false)
            {
                SetFullscreen(!Screen.fullScreen);
            }
        }
    }

    public void QuitGame()
    {
        if (ballMovement != null)
        {
            ballMovement.StopBall();
        }
        StartCoroutine(QuitAfterSound());
    }

    private IEnumerator QuitAfterSound()
    {
        PlaySoundEffect(quitGameClip, 0.3f);
        yield return new WaitForSeconds(quitGameClip.length);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetFullscreen(bool fullscreen)
    {
        if (fullscreen)
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
    }

    void ReadScore()
    {
        if (PlayerPrefs.HasKey("highScore"))
        {
            minimunTime = PlayerPrefs.GetInt("highScore");
            if (minimunTime == 0)
            {
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
