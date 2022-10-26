using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//So, this gets reaaaal buggy working just with OnTriggerEnter/Exit, so I've got the backup here
public class PressurePlate : Interactable
{
    [SerializeField] GameObject target; //door it opens, or other stuff
    bool inMotion = false; //pauses use while true
    public override void interact()
    {
        base.interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("enter");
        if (other.tag == "Player")
        {
            target.GetComponent<Animator>().SetBool("Open", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("exit");
        if (other.tag == "Player")
        {
            target.GetComponent<Animator>().SetBool("Open", false);
        }
    }
}
