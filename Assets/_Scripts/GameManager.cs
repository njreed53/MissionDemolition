using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject playAgainButton;
    public Slingshot slingshot;
    void Start()
    {
        gameOverUI.SetActive (false);
        playAgainButton.SetActive (false);
    }

    public void GameOver()
    {
        
        gameOverUI.SetActive(true);
        playAgainButton.SetActive (true);
        slingshot.enabled = false;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}