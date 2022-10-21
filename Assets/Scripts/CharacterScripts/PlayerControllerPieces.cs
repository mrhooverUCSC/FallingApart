using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//keep track of pieces, and how to control a different piece
public class PlayerControllerPieces : PlayerController
{
    //Copy of the body
    [Header("Pieces")]
    [SerializeField] Body parts; //keeps track off the pieces
    [SerializeField] GameObject controlPart;
    bool controlPartInUse; 

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(1) && controlPart != null)
        {
            controlPartInUse = !controlPartInUse;
            Debug.Log(controlPartInUse);
        }
    }

    protected override void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (controlPartInUse)
        {
            if (grounded) { controlPart.GetComponent<CharacterController>().SimpleMove(moveDir.normalized * moveSpeed * 10f); }
            else if (!grounded) { controlPart.GetComponent<CharacterController>().SimpleMove(moveDir.normalized * moveSpeed * 10f * airMultiplier); }
            if (grounded) { GetComponent<CharacterController>().SimpleMove(Vector3.zero); } //Character controller does not like having no input for some reason
            else if (!grounded) { GetComponent<CharacterController>().SimpleMove(Vector3.zero); }
        }
        else
        {
            if (grounded) { GetComponent<CharacterController>().SimpleMove(moveDir.normalized * moveSpeed * 10f); }
            else if (!grounded) { GetComponent<CharacterController>().SimpleMove(moveDir.normalized * moveSpeed * 10f * airMultiplier); }
        }
    }

    public override void interact(Interactable i) //need to check if you have the right pieces to interact with it
    {
        if (!parts.partAvailable(i.partNeeded) && !i.partInUse)
        {
            Debug.Log("Body Part not available " + i.partNeeded);
            return;
        }
        else if(!parts.partSpaceAvailable(i.partNeeded) && i.partInUse)
        {
            Debug.Log("no space for Body Part " + i.partNeeded);
            return;
        }
        if (!i.partInUse)
        {
            parts.usePart(i.partNeeded);
        }
        else
        {
            parts.addPart(i.partNeeded);
        }
        base.interact(i);
    }
}
