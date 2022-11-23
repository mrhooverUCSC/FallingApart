using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//So, this gets reaaaal buggy working just with OnTriggerEnter/Exit, so I've got the backup here
public class PressurePlate : Interactable
{
    [SerializeField] GameObject target; //door it opens, or other stuff
    //bool inMotion = false; //pauses use while true
    int numObjects = 0; //if there's multiple things, only disable when there are no things at all left
    Dictionary<string, UnityAction> listeners = new Dictionary<string, UnityAction>(); //need to keep a copy of the listener made to destroy later. 
                 //This Dictionary is likely overkill, but the other option was a "left pair" and a "right pair", so I made a more abstract version with higher functionality.
                 //Basically, when a listener is made, write down the name of the object with its function to use later, then delete it after using it.
    UnityAction trigger;
    public override void interact()
    {
        base.interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Player" || other.tag == "PlayerPiece"))
        {
            //Debug.Log("enter " + other.name + " | " + numObjects);
            target.GetComponent<Animator>().SetBool("Open", true);
            numObjects++;
            if(other.tag == "PlayerPiece") //only do this if it's a body part
            {
                //create listener to disable pressureplate if object destroyed and save in trigger
                other.gameObject.GetComponent<Interactable>().onDestroyEvent.AddListener(trigger = () => ObjectDestroyed());
                //when the listener is triggered, the dictionary isn't deleted, so just in case check if it's already there
                if (listeners.ContainsKey(other.name)) 
                {
                    listeners[other.name] = trigger;
                }
                else
                {
                    listeners.Add(other.name, trigger); //add the listener to the List as the name with the action
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player" || other.tag == "PlayerPiece"))
        {
            //Debug.Log("exit " + other.name + " | " + numObjects);
            numObjects--;
            if (other.tag == "PlayerPiece") //only do this if it's a body part
            {
                other.gameObject.GetComponent<Interactable>().onDestroyEvent.RemoveListener(listeners[other.name]); //remove listener if it leaves
                listeners.Remove(other.name);
                //Debug.Log(listeners.Count);
            }
            if (numObjects == 0)
            {
                target.GetComponent<Animator>().SetBool("Open", false);
            }
        }
    }
    private void ObjectDestroyed()
    {
        //Debug.Log("destroyed");
        numObjects--;
        if (numObjects == 0)
        {
            target.GetComponent<Animator>().SetBool("Open", false);
        }
    }
}
