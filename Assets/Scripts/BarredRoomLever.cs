using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inheireted from Interactable:
//    public BodyPart partNeeded; //assign in editor
//    public bool partInUse; //false if no body part on it, true if there is a body part on it
public class BarredRoomLever : Interactable
{
    bool inMotion = false; //pauses use while true
    [SerializeField] BarredRoomManager manager;
    [SerializeField] bool onLeft;
    [SerializeField] int registerNumber;
    [SerializeField] bool startActivated;

    public void Start()
    {
        if (startActivated)
        {
            interact();
        }
    }

    public override void interact() //move lever, if not in use, and marks body part used
    {
        Debug.Log("interact with barredroomlever");
        if (inMotion == false){
            if (!partInUse)
            {
                GetComponent<Animator>().SetBool("Used", true);
                partInUse = true;
            }
            else if(partInUse)
            {
                GetComponent<Animator>().SetBool("Used", false);
                partInUse = false;
            }
            inMotion = true;
        }
    }
    public void openDoor() //opens the door, gets called by the animation
    {
        Debug.Log("arm");
        if (onLeft)
        {
            manager.leftBar(true, registerNumber);
        }
        else
        {
            manager.rightBar(true, registerNumber);
        }
        inMotion = false;
    }
    public void closeDoor()
    {
        Debug.Log("unarm");
        if (onLeft)
        {
            manager.leftBar(false, registerNumber);
        }
        else
        {
            manager.rightBar(false, registerNumber);
        }
        inMotion = false;
    }
}
