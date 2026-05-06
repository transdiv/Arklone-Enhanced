using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class TimerHolder : MonoBehaviour
{
    public float timer = 0;
    public bool timerOn = false;
    public float minimunTime = 999999f;
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        //timerOn = true;
        ReadScore();
        SceneManager.LoadScene("StartScreen");
        //SceneManager.LoadScene("Level5");
        //SceneManager.LoadScene("EndScreen");

    }

    void Update()
    {
        if (timerOn == true)
        {
            timer += Time.deltaTime;
        }
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
    public void SaveScore()
    {
        if (timer < minimunTime)
        { 
            minimunTime = timer;
            PlayerPrefs.SetFloat("highScore", minimunTime);
            PlayerPrefs.Save();
        }
    }
}

