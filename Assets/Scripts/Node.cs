using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {

    public List<GameObject> players;

    public float radius;
    public float power;

    public float timeToDelete = 5;
    public float timeAlive = 0;

	void Update ()
    {
        timeAlive += Time.deltaTime;

        if (players.Count != 0)
        {
            foreach (GameObject p in players)
            {
                if (p != null)
                {
                    p.GetComponent<Player>().detectNode(this);
                }
            }
        }
	}
}
