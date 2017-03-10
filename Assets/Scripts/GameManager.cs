using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;


public class GameManager : MonoBehaviour
{
    public AudioScript audioScript;

    int maxLives = 3;
    [HideInInspector] public int currLives;
    [HideInInspector] public int currPlayersOnScreen;

    public GameObject iconPanel;
    public GameObject repulsePanel;
    public Texture lifeIcon;
    //public float spriteOffset;
    
    //public Slider radiusSlider;
    public Slider powerSlider;
    public Slider timeSlider;

    public Slider radiusCostSlider;
    public Slider powerCostSlider;

    public GameObject nodePrefab;
    public GameObject playerPrefab;
    public GameObject destPrefab;

    List<GameObject> nodes = new List<GameObject>();
    List<GameObject> players = new List<GameObject>();
    List<Image> icons = new List<Image>();
    List<Image> repulseIcons = new List<Image>();
    
    //List<Wall> walls = new List<Wall>();

    public float removeRadius = .00001f;
    public float lifespan = 5;

    //int totalPlayers = 1;
    public List<Transform> playerSpawns = new List<Transform>();
    public List<Transform> destSpawns = new List<Transform>();
    [HideInInspector] public List<GameObject> destinations = new List<GameObject>();

    public int totalNodes = 3;
    int currNodesOnMap = 0;

    public float maxTimeAmount = 10;
    [HideInInspector] public float timeAmount = 10;
    public float timeCostRate = 1;

    public float maxPowerPool = 10;
    public float powerPool = 10; //Total power one can spend
    public float maxCostPower = 5;  //max cost
    public float minCostPower = 1;  //min cost
    public float incPower = 1;  //How much the arrow keys increment power cost
    public float costPower = 2; //How much power it currently costs to place node

    //public float radiusPool = 10;
    //public float maxCostRadius = 5;
    //public float minCostRadius = 1;
    //public float incRadius = 1;
    public float costRadius = 2;

    bool paused = false;

    public float baseNodeRadius;  //Delete this later
    public float baseNodePower;   //Delete this later




    void Awake()
    {
        currPlayersOnScreen = playerSpawns.Count;
        currLives = maxLives;

        powerPool = maxPowerPool;
        powerSlider.maxValue = maxPowerPool;
        powerSlider.value = maxPowerPool;
        powerCostSlider.maxValue = maxPowerPool;

        timeSlider.maxValue = maxTimeAmount;
        timeSlider.value = timeAmount;
        //radiusSlider.maxValue = radiusPool;
        //radiusSlider.value = radiusPool;
        //radiusCostSlider.maxValue = radiusPool;

        createDestinations();
        addPlayers();
        icons = iconPanel.GetComponentsInChildren<Image>().ToList<Image>();
        repulseIcons = repulsePanel.GetComponentsInChildren<Image>().ToList<Image>();
    }

    void Update()
    {
        if (Input.GetButtonDown("PlaceNode")) addNode(baseNodeRadius, baseNodePower);
        if (Input.GetButtonDown("DeleteNode")) deleteNode();
        updateIcons();
        checkWhenNoPlayers();
        handlePause();
        handlePowerAndRadiusInput();
    }

    //void OnGUI()
    //{
    //GUI.DrawTexture(new Rect(10, 10, 100, 100), lifeIcon, ScaleMode.ScaleToFit, true, 10.0F);
    //}

    void updateIcons()
    {
        foreach (Image i in icons)
        {
            i.enabled = false;
        }

        for (int i = 0; i <= currLives; i++)
        {
            if (i < icons.Count)
            {
                icons[i].enabled = true;
            }
        }

        foreach (Image i in repulseIcons)
        {
            i.enabled = false;
        }

        for (int i = 0; i <= totalNodes - currNodesOnMap; i++)
        {
            if (i < repulseIcons.Count)
            {
                repulseIcons[i].enabled = true;
            }
        }
    }

    void checkWhenNoPlayers()
    {
        if (currPlayersOnScreen <= 0)
        {
            int totalNodesCollected = 0;

            foreach (GameObject dest in destinations)
            {
                totalNodesCollected += dest.GetComponent<Destination>().playersCollected;
                dest.GetComponent<Destination>().playersCollected = 0;
            }

            //If all the nodes were collected
            if (totalNodesCollected == playerSpawns.Count)
            {
               
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

                //Debug.Log("YOU WON");
            }
            //If some of the nodes were not collectec
            else //Resets the level, or goes to lose screen
            {
                currLives--;
                updateIcons();

                //If the player has lost the whole game
                if (currLives == 0)
                {
                    SceneManager.LoadScene("MainMenu");
                }
                //If player failed only once, but not too many
                //times in this level
                else
                {
                    addPlayers();
                    deleteAllNodes();
                    powerPool = maxPowerPool;
                    timeAmount = maxTimeAmount;
                    currNodesOnMap = 0;
                    currPlayersOnScreen = playerSpawns.Count;
                    updateSliders();
                }
            }
        }
    }

    void deleteAllNodes()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] != null)
            {
                Destroy(nodes[i]);
            }
        }
    }

    void handlePause()
    {
        if (paused & timeAmount > 0)
        {
            timeAmount -= timeCostRate * Time.deltaTime;
            timeSlider.value = timeAmount;
        }

        if (timeAmount < 0)
        {
            Time.timeScale = 1;
        }

        if (Input.GetButtonDown("Pause")  &&  timeAmount > 0)
        {
            if (!paused)
            {
                Time.timeScale = .1f;
                paused = true;
            }
            else
            {
                Time.timeScale = 1;
                paused = false;
            }
        }
    }

    void handlePowerAndRadiusInput()
    {
        if (Input.GetButtonDown("ChangePower")) costPower += Input.GetAxisRaw("ChangePower") * incPower;
        if (costPower < minCostPower) costPower = minCostPower;
        if (costPower > maxCostPower) costPower = maxCostPower;

        //if (Input.GetButtonDown("ChangeRadius")) costRadius += Input.GetAxisRaw("ChangeRadius") * incRadius;
        //if (costRadius < minCostRadius) costRadius = minCostRadius;
        //if (costRadius > maxCostRadius) costRadius = maxCostRadius;

        updateSliders();
    }

    void createDestinations()
    {
        for (int i = 0; i < destSpawns.Count; i++)
        {
            GameObject newDest = GameObject.Instantiate(destPrefab, destSpawns[i].position, Quaternion.identity) as GameObject;
            newDest.GetComponent<Destination>().gm = this;
            destinations.Add(newDest);
        }
    }

    void addPlayers()
    {
        foreach (Transform t in playerSpawns)
        {
            addPlayer(t.position);
        }
    }

    public void addPlayer(Vector2 spawn)
    {
        GameObject newPlayer = GameObject.Instantiate(playerPrefab, spawn, Quaternion.identity) as GameObject;
        newPlayer.GetComponent<Player>().gm = this;
        newPlayer.GetComponent<Player>().startingLoc = spawn;
        players.Add(newPlayer);
    }

    void addNode(float radius, float power)
    {
        if (currNodesOnMap < totalNodes)
        {
            if (powerPool > costPower /*&& radiusPool > costRadius*/)
            {
                Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);

                GameObject newNode = GameObject.Instantiate(nodePrefab, pos, Quaternion.identity) as GameObject;
                newNode.GetComponent<Node>().radius = costRadius;
                newNode.GetComponent<Node>().power = costPower;
                newNode.GetComponent<Node>().timeToDelete = costPower;
                newNode.GetComponent<Node>().players = players;

                nodes.Add(newNode);
                currNodesOnMap++;

                powerPool -= costPower;
            }
        }

        updateSliders();
    }

    void deleteNode()
    {
        if (currNodesOnMap > 0)
        {
            Debug.Log(nodes.Count);
            //Foreach node in nodes, delete the node if it is close enough
            for (int i = 0; i < nodes.Count; i++)
            {
                Camera cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                //Debug.Log(Vector2.Distance(nodes[i].gameObject.transform.position, cam.ScreenToWorldPoint(Input.mousePosition)));
                if (nodes[i] != null)
                {
                    if (nodes[i].GetComponent<Node>().timeAlive > nodes[i].GetComponent<Node>().timeToDelete)
                    {
                        if (Vector2.Distance(nodes[i].gameObject.transform.position, cam.ScreenToWorldPoint(Input.mousePosition)) < removeRadius)
                        {
                            powerPool += nodes[i].GetComponent<Node>().power;
                            //radiusPool += nodes[i].GetComponent<Node>().radius;

                            Destroy(nodes[i]);
                            currNodesOnMap--;
                        }
                    }
                }
            }
        }

        updateSliders();
    }

    void updateSliders()
    {
        powerSlider.value = powerPool;
        //radiusSlider.value = radiusPool;

        radiusCostSlider.value = costRadius;
        powerCostSlider.value = costPower;
    }
}
