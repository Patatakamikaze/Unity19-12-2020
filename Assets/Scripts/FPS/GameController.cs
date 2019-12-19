using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static int score;
    public GameObject fpscontroller;
    public Text finalScoreText;
    public Text inGameScoreText;
    public GameObject gameOverWindow;

    public bool gameOver { get; set; }

    private void Start() {
        gameOverWindow.SetActive(false);
        gameOver = false;
    }


    private void Update() {
        if (gameOver) {
            finalScoreText.text = "SCORE: " + score.ToString("00000");
            gameOverWindow.SetActive(true);

            if (Input.GetKey(KeyCode.Return)) {
                restartGame();
            }
        }
        else {
              inGameScoreText.text = "SCORE: " + score.ToString("00000");
        }    
    }

    public void AddPoints(int points) {
        score += points;
    }

    public static void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
	
}
