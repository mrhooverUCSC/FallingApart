using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum LeverType
{
    none = 0,
    Door
}
//inheireted from Interactable:
//    public BodyPart partNeeded; //assign in editor
//    public bool partInUse; //false if no body part on it, true if there is a body part on it
public class Lever : Interactable
{
    [SerializeField] LeverType type;
    bool inMotion = false; //pauses use while true
    [SerializeField] GameObject target;

    public override void interact() //move lever, if not in use, and marks body part used
    {
        if (inMotion == false){
            if (type == LeverType.Door)
            {
                GetComponent<Animator>().SetTrigger("Activate");
            }
            if (!partInUse)
            {
                partInUse = true;
            }
            else
            {
                partInUse = false;
            }
        }
        inMotion = true;
        
    }
    public void openDoor() //opens the door, gets called by the animation
    {
        target.GetComponent<Animator>().SetTrigger("Open");
        inMotion = false;
    }
}
