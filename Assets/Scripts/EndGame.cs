using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGame : MonoBehaviour
{
    public Button playGameButton;
    public Button levelSelectButton;
    public Button quitButton;

    void Start()
    {
        playGameButton.onClick.AddListener(playGame);
        levelSelectButton.onClick.AddListener(levelSelect);
        quitButton.onClick.AddListener(quit);
    }

    void playGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    void levelSelect()
    {

    }

    void quit()
    {
        Application.Quit();
    }
}
