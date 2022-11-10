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
    [SerializeField] LayerMask junctionMask;
    [SerializeField] LayerMask deadEndMask;
    [SerializeField] LayerMask wallMask;
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
            Debug.Log(transform.position + " | " + targetPosition);
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
                //Debug.Log("player in north");
                chooseDirection(direction.NORTH);
            }
            else if (Mathf.Abs(gameObject.transform.position.x - player.gameObject.transform.position.x) < 1.25 && gameObject.transform.position.z > player.gameObject.transform.position.z)
            {
                //Debug.Log("player in south");
                chooseDirection(direction.SOUTH);
            }
            else if (Mathf.Abs(gameObject.transform.position.z - player.gameObject.transform.position.z) < 1.25 && gameObject.transform.position.x < player.gameObject.transform.position.x)
            {
                //Debug.Log("player in east");
                chooseDirection(direction.EAST);
            }
            else if (Mathf.Abs(gameObject.transform.position.z - player.gameObject.transform.position.z) < 1.25 && gameObject.transform.position.x > player.gameObject.transform.position.x)
            {
                //Debug.Log("player in west");
                chooseDirection(direction.WEST);
            }
        }
    }
    //raycast in directions player is not in. find the best one, then go there.
    void chooseDirection(direction dir)
    {
        RaycastHit hit;
        Collider target = null;
        //if nothing is in the way, look for a junction, and save it if there is one.
        if (dir != direction.NORTH && !Physics.Raycast(transform.position, North, out hit, 25, wallMask))
        {
            Debug.DrawRay(transform.position, North * 25, Color.cyan, .5f, true);
            if(Physics.Raycast(transform.position, North, out hit, 25, junctionMask))
            {
                Debug.Log("junction to north");
                target = hit.collider;
            }
        }
        if (dir != direction.SOUTH && !Physics.Raycast(transform.position, South, out hit, 25, wallMask))
        {
            Debug.DrawRay(transform.position, South * 25, Color.cyan, .5f, true);
            if (Physics.Raycast(transform.position, South, out hit, 25, junctionMask))
            {
                Debug.Log("junction to south");
                target = hit.collider;
            }
        }
        if (dir != direction.EAST && !Physics.Raycast(transform.position, East, out hit, 25, wallMask))
        {
            Debug.DrawRay(transform.position, East * 25, Color.cyan, .5f, true);
            if (Physics.Raycast(transform.position, East, out hit, 25, junctionMask))
            {
                Debug.Log("junction to east");
                target = hit.collider;
            }
        }
        if (dir != direction.WEST && !Physics.Raycast(transform.position, West, out hit, 25, wallMask))
        {
            Debug.DrawRay(transform.position, West * 25, Color.cyan, .5f, true);
            if (Physics.Raycast(transform.position, West, out hit, 25, junctionMask))
            {
                Debug.Log("junction to west");
                target = hit.collider;
            }
        }
        //if you found a junction, go to it
        if (target != null)
        {
            Debug.Log(target.name);
            running = true;
            targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            Debug.Log(targetPosition);
        }
        //otherwise, repeat for deadends instead of junctions
    }
}
