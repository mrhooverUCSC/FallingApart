using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WolfScript : MonoBehaviour
{
    private class junctionInfo { //for the junction analysis in chooseDirection
        public Collider collider;
        public direction dir;
        public int priority;
        public junctionInfo(Collider c, direction d, int p)
        {
            priority = p;
            dir = d;
            collider = c;
        }
        public override string ToString()
        {
            return dir + " " + collider + " " + priority;
        }
    } 
    //Cardinal directions: 
    public static readonly Vector3 North = new Vector3(0, 0, 1);
    public static readonly Vector3 South = new Vector3(0, 0, -1);
    public static readonly Vector3 East = new Vector3(1, 0, 0);
    public static readonly Vector3 West = new Vector3(-1, 0, 0);
    //rayacsting
    int wallPlayerMask = 0b100000011000000; //layermask of wall and raycastableplayer
    Vector3 origin; //the wuf's origin is at the bottom of its model, so the 'origin' gets from the waist instead for more consistent raycasts. needs to be updated in Update()
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
    bool cornered = false; //once it hits a dead end, trigger this
    Vector3 targetPosition;
    CharacterController cController;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (activated)
        {
            origin = gameObject.transform.position + new Vector3(0, .2f, 0); //setup the origin for raycasting
            detectPlayer();
            run();
        }
    }
    //Controls all the actual movements
    private void run()
    {
        if (running && Vector3.Distance(transform.position, targetPosition) > .4f)
        {
            GetComponent<Animator>().SetBool("Running", true);
            cController.SimpleMove((targetPosition - transform.position).normalized * 6); //move towards the point
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position).normalized, Time.deltaTime * 2); //turn towards it
        }
        else if (running)
        {
            Debug.Log("wolf arrived at location");
            running = false;
        }
        if (!running && cornered)
        {
            //drop the leg and leave
            GetComponent<Animator>().SetBool("Drop", true);
        }
        else if (!running)//if not running around look at the player
        {
            GetComponent<Animator>().SetBool("Running", false);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position).normalized, Time.deltaTime * 2); //turn towards it
        }

    }
    //detect if player is in line and visible, then send that over to chooseDirection
    //if wall in the way, then don't run away. the player has a secondary hitbox for this reason, so the wolf can see the player fully from any place
    private void detectPlayer()
    {
        if (activated && !running && !cornered)
        {
            RaycastHit hit;
            if (Mathf.Abs(gameObject.transform.position.x - player.gameObject.transform.position.x) < 1 //if in line
                && gameObject.transform.position.z < player.gameObject.transform.position.z //and in this direction
                && Physics.Raycast(origin, North, out hit, 15, 0b100000010000000)) //check for the collision of a wall or the player
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("raycastablePlayer")) //if it's the player
                {
                    Debug.DrawRay(origin, North * 15, Color.green, .5f);
                    Debug.Log("player in north");
                    chooseDirection(direction.NORTH);
                }
            }
            else if (Mathf.Abs(gameObject.transform.position.x - player.gameObject.transform.position.x) < 1
                && gameObject.transform.position.z > player.gameObject.transform.position.z
                && Physics.Raycast(origin, South, out hit, 15, 0b100000010000000))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("raycastablePlayer"))
                {
                    Debug.DrawRay(origin, South * 15, Color.green, .5f);
                    Debug.Log("player in south");
                    chooseDirection(direction.SOUTH);
                }
            }
            else if (Mathf.Abs(gameObject.transform.position.z - player.gameObject.transform.position.z) < 1
                && gameObject.transform.position.x < player.gameObject.transform.position.x
                && Physics.Raycast(origin, East, out hit, 15, 0b100000010000000))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("raycastablePlayer"))
                {
                    Debug.DrawRay(origin, East * 15, Color.green, .5f);
                    Debug.Log("player in east");
                    chooseDirection(direction.EAST);
                }
            }
            else if (Mathf.Abs(gameObject.transform.position.z - player.gameObject.transform.position.z) < 1
                && gameObject.transform.position.x > player.gameObject.transform.position.x
                && Physics.Raycast(origin, West, out hit, 15, 0b100000010000000))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("raycastablePlayer"))
                {
                    Debug.DrawRay(origin, West * 15, Color.green, .5f);
                    Debug.Log("player in west");
                    chooseDirection(direction.WEST);
                }
            }
        }
    }
    //raycast in directions player is not in. find the best one, then go there.
    private void chooseDirection(direction dir) //dir is direction the player is in
    {
        Debug.Log("---- STARTING CHOOSEDIRECTION ----");
        RaycastHit hit;
        List<junctionInfo> junctions = new List<junctionInfo>(); //a list to keep track of what the wolf finds, unsorted
        if (dir != direction.NORTH)
        {
            Debug.Log("checking north");
            Debug.DrawRay(origin, North * 25, Color.cyan, .5f, true);
            if (Physics.Raycast(origin, North, out hit, 25, 0b110000000000000))
            {
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("junction"))
                {
                    Debug.Log("found junction priority " + hit.collider.gameObject.tag + " to north");
                    junctions.Add(new junctionInfo(hit.collider, direction.NORTH, hit.collider.gameObject.tag[hit.collider.gameObject.tag.Length - 1]));
                }
            }
        }
        if (dir != direction.SOUTH)// && !Physics.Raycast(transform.position, South, out hit, 25, ~wallMask))
        {
            Debug.Log("checking south");
            Debug.DrawRay(origin, South * 25, Color.cyan, .5f, true);
            if (Physics.Raycast(origin, South, out hit, 25, 0b110000000000000))
            {
                Debug.Log(hit.collider.gameObject.name + " | " + hit.collider.gameObject.tag);
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("junction"))
                {

                    Debug.Log("found junction priority " + hit.collider.gameObject.tag + " to south");
                    junctions.Add(new junctionInfo(hit.collider, direction.SOUTH, hit.collider.gameObject.tag[hit.collider.gameObject.tag.Length - 1]));
                }
            }
        }
        if (dir != direction.EAST)// && !Physics.Raycast(transform.position, East, out hit, 25, ~wallMask))
        {
            Debug.Log("checking east");
            Debug.DrawRay(origin, East * 25, Color.cyan, .5f, true);
            if (Physics.Raycast(origin, East, out hit, 25, 0b110000000000000))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("junction"))
                {
                    Debug.Log("found junction priority " + hit.collider.gameObject.tag + " to east");
                    junctions.Add(new junctionInfo(hit.collider, direction.EAST, hit.collider.gameObject.tag[hit.collider.gameObject.tag.Length - 1]));
                }
            }
        }
        if (dir != direction.WEST)// && !Physics.Raycast(transform.position, West, out hit, 25, ~wallMask))
        {
            Debug.Log("checking west");
            Debug.DrawRay(origin, West * 25, Color.cyan, .5f, true);
            if (Physics.Raycast(origin, West, out hit, 25, 0b110000000000000))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("junction"))
                {
                    Debug.Log("found junction priority " + hit.collider.gameObject.tag + " to west");
                    junctions.Add(new junctionInfo(hit.collider, direction.WEST, hit.collider.gameObject.tag[hit.collider.gameObject.tag.Length - 1]));
                }
            }
        }
        //if you found a junction, go to the highest prority
        foreach(junctionInfo i in junctions)
        {
            Debug.Log(i.ToString());
        }
        if (junctions.Count > 0)
        {
            for(int i = 49; i < 53; ++i) //ascii 1-4
            {
                foreach(junctionInfo j in junctions)
                {
                    if(j.priority == i)
                    {
                        Debug.Log(j.ToString());
                        running = true;
                        if(i == 52)
                        {
                            cornered = true;
                        }
                        targetPosition = new Vector3(j.collider.transform.position.x, transform.position.y, j.collider.transform.position.z);
                        Debug.Log(targetPosition);
                        return;
                    }
                }
            }
        }
        Debug.LogError("Failed to find any junctions for Wolf1 from player in direction " + dir);
    }

    public void dropLeg()
    {
        GameObject temp = Instantiate(Resources.Load<GameObject>("Prefabs/RightLegDrop"));
        temp.transform.position = transform.position + new Vector3(0, .3f, 0);
    }

    public void leave()
    {
        Destroy(this.gameObject);
    }
}
