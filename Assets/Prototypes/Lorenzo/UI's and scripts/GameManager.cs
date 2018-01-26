using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    bool gameIsOver = false;
    public float gameOverDelay = 2f;
    public GameObject gameOverMenuUI;
    public Text Text_score;



    public void GameOver() {
        if (gameIsOver == false)
        {
            gameIsOver = true;
            Invoke("GameOverScreen", gameOverDelay);
            int points = ScoreManager.score;
            Text_score.text = "score: " + points.ToString();
        }
    }

    void GameOverScreen()
    {
        Time.timeScale = 0f;
        gameOverMenuUI.SetActive(true);
    }

}
