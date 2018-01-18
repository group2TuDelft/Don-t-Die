using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    bool gameIsOver = false;

    public float gameOverDelay = 2f;
    public GameObject gameOverMenuUI;
    public void GameOver()
    {
        if (gameIsOver == false)
        {
            gameIsOver = true;
            Invoke("GameOverScreen", gameOverDelay);
        }
    }

    void GameOverScreen()
    {
        Time.timeScale = 0f;
        gameOverMenuUI.SetActive(true);
    }

}
