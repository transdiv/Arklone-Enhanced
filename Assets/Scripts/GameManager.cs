using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    int blocksLeft;
    const int lastScene = 7;
    bool quit = false;
    int activeSceneIndex;
    TimerHolder timeScript;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] Image smiley;
    [SerializeField] AudioClip quitGameClip;
    [SerializeField] AudioClip levelEndClip;
    [SerializeField] AudioClip cheersClip;

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
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        blocksLeft = GameObject.FindGameObjectsWithTag("Block").Length;
        timeScript = GameObject.Find("TimerHolder").GetComponent<TimerHolder>();
        //timeScript.timerOn = true; // Provisional
        if (SceneManager.GetActiveScene().buildIndex == lastScene) 
        {
            score.text = string.Format(" {00000}", Math.Floor(timeScript.timer));
            if (timeScript.timer < timeScript.minimunTime)
            {
                smiley.gameObject.SetActive(true);
                AudioManager.Instance.PlaySoundEffect(cheersClip, 0.5f);
            }
            timeScript.SaveScore();
        }

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
        timeScript.timerOn = true;
        if (nextSceneIndex == lastScene)
        {
            timeScript.timerOn = false;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void Update()
    {
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

}
