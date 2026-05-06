using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    int blocksLeft;
    const int lastScene = 6;
    bool quit = false;
    int activeSceneIndex;

    [SerializeField] AudioClip quitGameClip;
    [SerializeField] AudioClip levelEndClip;
    [SerializeField] AudioClip cheersClip;

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
        Debug.Log($"Scene: {activeSceneIndex}, blocksLeft: {blocksLeft}");
        // End game logic goes here
        if (activeSceneIndex == lastScene)
        {
            TextMeshProUGUI score = GameObject.Find("scoreText")?.GetComponent<TextMeshProUGUI>();
            Image smiley = GameObject.Find("Smiley")?.GetComponent<Image>();
            timerOn = false;
            score.text = string.Format("{0:00000}", Math.Floor(timer));
            Debug.Log($"timer: {timer} , minimunTime: {minimunTime}");
            if (timer < minimunTime)
            {
                smiley.gameObject.SetActive(true);
                AudioManager.Instance.PlaySoundEffect(cheersClip, 0.5f);
                SaveScore((int) timer);
            }
        }
    }

    void Start()
    {
        ReadScore();
        Debug.Log($"minimunTime: {minimunTime}");
        SceneManager.LoadScene(5);
        timerOn = true;
    }

    
    public void RestartScene()
    {
        SceneManager.LoadScene(activeSceneIndex);
    }

    public void DecreaseBlock()
    {
        blocksLeft--;
        if (blocksLeft == 0)
        {
            AudioManager.Instance.PlaySoundEffect(levelEndClip, 0.5f);
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

        if (Input.GetKey("escape") && activeSceneIndex > 1 && activeSceneIndex < lastScene)
        {
            if (quit == false)
            {
                quit = true;
                QuitGame();
            }
        }
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlaySoundEffect(quitGameClip, 0.5f);
        Application.Quit();
    }

    void ReadScore()
    {
        if (PlayerPrefs.HasKey("highScore"))
        {
            minimunTime = PlayerPrefs.GetFloat("highScore");
            if (minimunTime == 0)
            {
                PlayerPrefs.SetFloat("highScore", 999999f);
                PlayerPrefs.Save();
                minimunTime = 999999f;
            }
        }
        else
        {
            minimunTime = 999999f;
            PlayerPrefs.SetFloat("highScore", minimunTime);
            PlayerPrefs.Save();
        }
    }
    public void SaveScore(int minimun)
    {
        PlayerPrefs.SetFloat("highScore", minimun);
        PlayerPrefs.Save();
    }

    public void ResetScore()
    {
        PlayerPrefs.SetFloat("highScore", (int) 999999);
        PlayerPrefs.Save();
    }

}
