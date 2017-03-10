using UnityEngine;
using System.Collections;

public class camMove : MonoBehaviour
{
    public GameObject player;
    public float maxSpeed = 2;
    public float maxAccel = 2;
    public Vector2 velocity = new Vector2(0, 0);
    public Vector2 acceleration = new Vector2(0, 0);

    public Rect deadRec; 
    float arriveDist = 2;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector2 coord2D = GetComponent<Camera>().WorldToScreenPoint(player.gameObject.transform.position);
        //if (deadRec.Contains(coord2D) == false)
        {
            addSteerToAccel(player.gameObject.transform.position);
        }
    }

    //Moves the camera towards the player position with smoothing
    void addSteerToAccel(Vector2 destination)
    {
        Vector2 desired = destination - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        float d = desired.magnitude;

        if (d < arriveDist)
        {
            float m = scale(0, arriveDist, 0, maxSpeed, d);
            desired *= m;
            Debug.Log(m);
        }
        else
        {
            desired *= maxSpeed;
        }

        Vector2 steer = desired - velocity;
        steer = Vector2.ClampMagnitude(steer, maxAccel);

        GetComponent<Rigidbody>().AddForce(steer);
    }

    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
}

