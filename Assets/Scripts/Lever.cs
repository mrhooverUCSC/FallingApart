using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum LeverType
{
    none = 0,
    Door,
    WolfDoor,
    Bar
}
//inheireted from Interactable:
//    public BodyPart partNeeded; //assign in editor
//    public bool partInUse; //false if no body part on it, true if there is a body part on it
public class Lever : Interactable
{
    [SerializeField] LeverType type;
    bool inMotion = false; //pauses use while true
    [SerializeField] GameObject target;
    [SerializeField] GameObject target2;
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
        //Debug.Log("open door called");
        if(type == LeverType.Door)
        {
            target.GetComponent<Animator>().SetBool("Open", true);
        }
        if (type == LeverType.WolfDoor) //wolf doors are open by default, and close when interacted with
        {
            target.GetComponent<Animator>().SetBool("Closed", true);
            target2.GetComponent<Animator>().SetBool("Closed", true);
        }
        if(type == LeverType.Bar)
        {
            target.GetComponent<Animator>().SetBool("Retract", true);
            target2.GetComponent<BarredRoomManager>().mainBar(true, registerNumber);
        }
        inMotion = false;
    }
    public void closeDoor()
    {
        //Debug.Log("close door called");
        if (type == LeverType.Door)
        {
            target.GetComponent<Animator>().SetBool("Open", false);
        }
        if (type == LeverType.WolfDoor)
        {
            target.GetComponent<Animator>().SetBool("Closed", false);
            target2.GetComponent<Animator>().SetBool("Closed", false);
        }
        if (type == LeverType.Bar)
        {
            target.GetComponent<Animator>().SetBool("Retract", false);
            target2.GetComponent<BarredRoomManager>().mainBar(false, registerNumber);
        }
        inMotion = false;
    }
}
