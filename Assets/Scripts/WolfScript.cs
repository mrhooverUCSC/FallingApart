using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfScript : MonoBehaviour
{
    //Cardinal directions: 
    public static readonly Vector3 North = new Vector3(0, 0, 1);
    public static readonly Vector3 South = new Vector3(0, 0, -1);
    public static readonly Vector3 East = new Vector3(1, 0, 0);
    public static readonly Vector3 West = new Vector3(-1, 0, 0);
    enum direction
    {
        NORTH,
        SOUTH,
        EAST,
        WEST
    }
    //variables
    GameObject player;
    public bool activated = false;
    bool running = false; //so the wolf doesn't keep tracking while moving between points
    Vector3 targetPosition;
    CharacterController cController;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cController = GetComponent<CharacterController>();
    }

    void Update()
    {
        detectPlayer();
        if (running && Vector3.Distance(transform.position, targetPosition) > .2f)
        {
            //Debug.Log(transform.position + " | " + targetPosition);
            cController.SimpleMove((targetPosition - transform.position).normalized * 2);
        }
        else if(running)
        {
            running = false;
        }
    }
    //detect if player is in line, then send that over to chooseDirection
    private void detectPlayer()
    {
        if (activated && !running)
        {
            if (Mathf.Abs(gameObject.transform.position.x - player.gameObject.transform.position.x) < 1.25 && gameObject.transform.position.z < player.gameObject.transform.position.z)
            {
                Debug.Log("player in north");
                chooseDirection(direction.NORTH);
            }
            else if (Mathf.Abs(gameObject.transform.position.x - player.gameObject.transform.position.x) < 1.25 && gameObject.transform.position.z > player.gameObject.transform.position.z)
            {
                Debug.Log("player in south");
                chooseDirection(direction.SOUTH);
            }
            else if (Mathf.Abs(gameObject.transform.position.z - player.gameObject.transform.position.z) < 1.25 && gameObject.transform.position.x < player.gameObject.transform.position.x)
            {
                Debug.Log("player in east");
                chooseDirection(direction.EAST);
            }
            else if (Mathf.Abs(gameObject.transform.position.z - player.gameObject.transform.position.z) < 1.25 && gameObject.transform.position.x > player.gameObject.transform.position.x)
            {
                Debug.Log("player in west");
                chooseDirection(direction.WEST);
            }
        }
    }
    //raycast in directions player is not in. find the best one, then go there.
    void chooseDirection(direction dir) //dir is direction the player is in
    {
        Debug.Log("---- STARTING CHOOSEDIRECTION ----");
        RaycastHit hit;
        //Layers are just a number, so looping is super easy. I'm using 'i' just for readability in the actual loop, as a hold over for "Layers 13-16"
        LayerMask wallMask = LayerMask.NameToLayer("wall");
        LayerMask temp = LayerMask.NameToLayer("junctionP1"); //iteratable layermask
        List<KeyValuePair<Collider, direction>> junctions = new List<KeyValuePair<Collider, direction>>(); //a list to keep track of what the wolf finds, in order of prority
        //for each prority, check if there is that prority in each direction the player is not coming from, then save it if there is
        for (int i = 1; i < 4; ++i, ++temp){
            Debug.Log("-- Starting Layer " + i + " | " + LayerMask.LayerToName(temp));
            if (dir != direction.NORTH)// && !Physics.Raycast(transform.position, North, out hit, 25, wallMask))
            {
                Debug.Log("checking north");
                Debug.DrawRay(transform.position, North * 25, Color.cyan, .5f, true);
                if (Physics.Raycast(transform.position, North, out hit, 25, ~temp))
                {
                    Debug.Log("found junction priority " + i + " to north");
                    junctions.Add(new KeyValuePair<Collider, direction>(hit.collider, direction.NORTH));
                }
            }
            if (dir != direction.SOUTH)// && !Physics.Raycast(transform.position, South, out hit, 25, wallMask))
            {
                Debug.Log("checking south");
                Debug.DrawRay(transform.position, South * 25, Color.cyan, .5f, true);
                if (Physics.Raycast(transform.position, South, out hit, 25, ~temp))
                {
                    Debug.Log("found junction priority " + i + " to south");
                    junctions.Add(new KeyValuePair<Collider, direction>(hit.collider, direction.SOUTH));
                }
            }
            if (dir != direction.EAST)// && !Physics.Raycast(transform.position, East, out hit, 25, wallMask))
            {
                Debug.Log("checking east");
                Debug.DrawRay(transform.position, East * 25, Color.cyan, .5f, true);
                if (Physics.Raycast(transform.position, East, out hit, 25, ~temp))
                {
                    Debug.Log("found junction priority " + i + " to east");
                    junctions.Add(new KeyValuePair<Collider, direction>(hit.collider, direction.EAST));
                }
            }
            if (dir != direction.WEST)// && !Physics.Raycast(transform.position, West, out hit, 25, wallMask))
            {
                Debug.Log("checking west");
                Debug.DrawRay(transform.position, West * 25, Color.cyan, .5f, true);
                if (Physics.Raycast(transform.position, West, out hit, 25, ~temp))
                {
                    Debug.Log("found junction priority " + i + " to west");
                    junctions.Add(new KeyValuePair<Collider, direction>(hit.collider, direction.WEST));
                }
            }
        }
        //if you found a junction, go to the first one found
        foreach(KeyValuePair<Collider, direction> i in junctions)
        {
            Debug.Log(i);
        }
        if (junctions.Count > 0)
        {
            Debug.Log(junctions[0]);
            running = true;
            targetPosition = new Vector3(junctions[0].Key.transform.position.x, transform.position.y, junctions[0].Key.transform.position.z);
            Debug.Log(targetPosition);
            return;
        }
        Debug.LogError("Failed to find any junctions for Wolf1 from player in direction " + dir);
    }
}
