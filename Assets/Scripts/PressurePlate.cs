using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//So, this gets reaaaal buggy working just with OnTriggerEnter/Exit, so I've got the backup here
public class PressurePlate : Interactable
{
    [SerializeField] GameObject target; //door it opens, or other stuff
    bool inMotion = false; //pauses use while true
    int numObjects = 0; //if there's multiple things, only disable when there are no things at all left
    public override void interact()
    {
        base.interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("enter");
        if ((other.tag == "Player" || other.tag == "PlayerPiece"))
        {
            target.GetComponent<Animator>().SetBool("Open", true);
            numObjects++;
            if(other.tag == "PlayerPiece") //only do this if it's a body part
            {
                other.gameObject.GetComponent<Interactable>().onDestroyEvent.AddListener(() => ObjectDestroyed()); //create listener to disable pressureplate if object destroyed
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("exit");
        if ((other.tag == "Player" || other.tag == "PlayerPiece"))
        {
            numObjects--;
            if (other.tag == "PlayerPiece") //only do this if it's a body part
            {
                other.gameObject.GetComponent<Interactable>().onDestroyEvent.RemoveListener(() => ObjectDestroyed()); //remove listener if it leaves
            }
            if (numObjects == 0)
            {
                target.GetComponent<Animator>().SetBool("Open", false);
            }
        }
    }
    private void ObjectDestroyed()
    {
        numObjects--;
        if (numObjects == 0)
        {
            target.GetComponent<Animator>().SetBool("Open", false);
        }
    }
}
