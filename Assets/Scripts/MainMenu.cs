using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public Button playGameButton;
    public Button instructionsButton;
    public Button quitButton;

    void Start()
    {
        playGameButton.onClick.AddListener(playGame);
        instructionsButton.onClick.AddListener(instructions);
        quitButton.onClick.AddListener(quit);
    }

    void playGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    void instructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    void quit()
    {
        Application.Quit();
    }
}
