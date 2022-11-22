using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WolfScript : MonoBehaviour
{
    private class junctionInfo{
        public Collider collider;
        public direction dir;
        public int priority;
        public junctionInfo(Collider c, direction d, int p)
        {
            priority = p;
            dir = d;
            collider = c;
        }
        public string ToString()
        {
            return dir + " " + collider + " " + priority;
        }
    } //for the junction analysis in chooseDirection
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
        int temp = 12; //the junction layermask
        int layermask = 1 << temp; //bit shifting it
        /*THERE IS SOMETHING FUNDAMENTALLY WRONG HERE
         * I was using NameToLayer("junction"), but that doesn't work was i wanted it to
         * it's taking the layermask, but temp is "1101" so ~temp is "11111111111111111111111111110010".
         * No idea why it's being saved as the number, not as the bit string, but that makes it useless for inversions
         * I don't know why the layermask is being set like that, so instead I'm doing it manually, to get "10000000000000" -> "11111111111111111101111111111111"
         * Just, like, gottta be careful with it.
         * Oh, and my theory is this only messes with "~" because when I use it elsewhere without ~ it works like I expect it to.
         */
        Debug.Log(System.Convert.ToString(layermask, 2) + " " + System.Convert.ToString((~layermask), 2));
        List<junctionInfo> junctions = new List<junctionInfo>(); //a list to keep track of what the wolf finds, unsorted
        if (dir != direction.NORTH)
        {
            Debug.Log("checking north");
            Debug.DrawRay(transform.position, North * 25, Color.cyan, .5f, true);
            if (Physics.Raycast(transform.position, North, out hit, 25, ~layermask))
            {
                Debug.Log("found junction priority " + hit.collider.gameObject.tag + " to north");
                junctions.Add(new junctionInfo(hit.collider, direction.NORTH, hit.collider.gameObject.tag[hit.collider.gameObject.tag.Length-1]));
            }
        }
        if (dir != direction.SOUTH)// && !Physics.Raycast(transform.position, South, out hit, 25, ~wallMask))
        {
            Debug.Log("checking south");
            Debug.DrawRay(transform.position, South * 25, Color.cyan, .5f, true);
            if (Physics.Raycast(transform.position, South, out hit, 25, ~layermask))
            {
                Debug.Log("found junction priority " + hit.collider.gameObject.tag + " to south");
                junctions.Add(new junctionInfo(hit.collider, direction.SOUTH, hit.collider.gameObject.tag[hit.collider.gameObject.tag.Length - 1]));
            }
        }
        if (dir != direction.EAST)// && !Physics.Raycast(transform.position, East, out hit, 25, ~wallMask))
        {
            Debug.Log("checking east");
            Debug.DrawRay(transform.position, East * 25, Color.cyan, .5f, true);
            if (Physics.Raycast(transform.position, East, out hit, 25, ~layermask))
            {
                Debug.Log("found junction priority " + hit.collider.gameObject.tag + " to east");
                junctions.Add(new junctionInfo(hit.collider, direction.EAST, hit.collider.gameObject.tag[hit.collider.gameObject.tag.Length - 1]));
            }
        }
        if (dir != direction.WEST)// && !Physics.Raycast(transform.position, West, out hit, 25, ~wallMask))
        {
            Debug.Log("checking west");
            Debug.DrawRay(transform.position, West * 25, Color.cyan, .5f, true);
            if (Physics.Raycast(transform.position, West, out hit, 25, ~layermask))
            {
                Debug.Log("found junction priority " + hit.collider.gameObject.tag + " to west");
                junctions.Add(new junctionInfo(hit.collider, direction.WEST, hit.collider.gameObject.tag[hit.collider.gameObject.tag.Length - 1]));
            }
        }
        //if you found a junction, go to the highest prority
        foreach(junctionInfo i in junctions)
        {
            Debug.Log(i.ToString());
        }
        if (junctions.Count > 0)
        {
            for(int i = 49; i < 54; ++i)
            {
                foreach(junctionInfo j in junctions)
                {
                    if(j.priority == i)
                    {
                        Debug.Log(j.ToString());
                        running = true;
                        targetPosition = new Vector3(j.collider.transform.position.x, transform.position.y, j.collider.transform.position.z);
                        Debug.Log(targetPosition);
                        return;
                    }
                }
            }
        }
        Debug.LogError("Failed to find any junctions for Wolf1 from player in direction " + dir);
    }
}
