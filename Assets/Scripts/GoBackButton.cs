using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoBackButton : MonoBehaviour {

    public Button goBackButton;

    void Start ()
    {
        goBackButton.onClick.AddListener(goBack);
	}
	
    void goBack()
    {
        SceneManager.LoadScene("MainMenu");
	}
}
