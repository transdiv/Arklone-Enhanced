using UnityEngine;

public class Menu : MonoBehaviour
{
    public void LoadNextScene()
    {
        GameManager.Instance.LoadNextScene();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
