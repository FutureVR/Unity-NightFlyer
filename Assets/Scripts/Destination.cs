using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class Destination : MonoBehaviour
{
    public GameManager gm;
    public int playersCollected = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            hitPlayer(other);
        }
    }

    void hitPlayer(Collider2D other)
    {
        gm.currPlayersOnScreen--;
        Destroy(other.gameObject);
        playersCollected++;
        //Debug.Log("HERE");
    }

    void winThisLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void loseThisLevel()
    {

    }
}
