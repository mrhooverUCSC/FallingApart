using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Mainly takes care of managing the maze connections.
//The wolf Ai doesn't need a real "path-finding" AI. All it needs is the direction the wolf is coming from, and a way to get nearby nodes.
//To that end, each "junction" will have a Box Collider that the wolf can detect, and run to away from the player.
public class WolfMazeLevel : MonoBehaviour
{
    bool activated = false;
    [SerializeField] WolfScript wolf;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !activated)
        {
            activated = true;
            Debug.Log("wolf area start");
            wolf.activated = true;
        }
    }
}
