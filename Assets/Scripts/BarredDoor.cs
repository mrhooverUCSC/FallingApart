using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inheireted from Interactable:
//    public BodyPart partNeeded; //assign in editor
//    public bool partInUse; //false if no body part on it, true if there is a body part on it

// this door checks to see if it can open, and then opens.
public class BarredDoor : Interactable
{
    [SerializeField] BarredRoomManager manager;

    public override void interact() //move lever, if not in use, and marks body part used
    {
        if (manager.checkDoor())
        {
            GetComponent<Animator>().SetBool("Open", true);
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Jiggle");
        }
    }
}
