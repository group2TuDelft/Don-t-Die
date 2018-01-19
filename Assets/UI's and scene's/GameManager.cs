using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    bool gameIsOver = false;
    public float gameOverDelay = 2f;
    public GameObject gameOverMenuUI;
    public GameObject point_sphere;
    public Text Text_score;
    private points_on_click points;


    public void GameOver() {
        if (gameIsOver == false)
        {
            gameIsOver = true;
            Invoke("GameOverScreen", gameOverDelay);
            points = point_sphere.GetComponent<points_on_click>();
            Text_score.text = "score: " + points.points;
        }
    }

    void GameOverScreen()
    {
        Time.timeScale = 0f;
        gameOverMenuUI.SetActive(true);
    }

}
