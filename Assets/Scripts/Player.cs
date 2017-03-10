using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gm;
    public Vector2 startingLoc;

    public int inventory;
    public Vector2 speed = new Vector2(0, 0);
    public Vector2 acceleration = new Vector2(0, 0);
    public float dragAcc = 2.0f; //constant
    public float dragSpeed = 2.0f; //constant

    [HideInInspector] Vector3 originalPos;

    // Use this for initialization
    void Awake()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = transform.position - originalPos;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            originalPos = transform.position;
        }
    }

    public void detectNode(Node n)
    {
        Vector2 distToPlayer = gameObject.transform.position - n.gameObject.transform.position;
        //Debug.Log(n.gameObject.transform.position);
        GetComponent<Rigidbody2D>().AddForce( calculateVector(n.radius, n.power, distToPlayer) );
    }

    public Vector2 calculateVector(float radius, float power, Vector2 distanceToPlayer)
    {
        //if (distanceToPlayer.magnitude > radius)
        {
            //return Vector2.zero;
        }

        //return distanceToPlayer;
        //Vector2 appliedAccel = new Vector2(distanceToPlayer.x * power / radius, distanceToPlayer.y * power / radius);
        float appliedAccel = (radius * power) / distanceToPlayer.magnitude;
        return distanceToPlayer.normalized * appliedAccel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall") hitWall();
        if (collision.gameObject.tag == "Destination") hitDestination();
    }

    private void hitWall()
    {
        gm.currPlayersOnScreen--;
        //gm.addPlayer(startingLoc);
       
        Destroy(this.gameObject);
    }

    private void hitDestination()
    {
        //Debug.Log("You won!");
    }
}