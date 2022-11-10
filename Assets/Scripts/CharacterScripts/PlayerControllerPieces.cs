using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//PlayerController with body part usage.
//Arms are used on objects by interacting with 'E'
//Legs are used with Left/Right click. One press disconnects them for control or re-controls them if already disconnected. 'E' reconnects them if in range.
public class PlayerControllerPieces : PlayerController
{
    //Copy of the body
    [Header("Pieces")]
    [SerializeField] Body parts; //keeps track off the pieces
    GameObject controlPartGO = null;
    BodyPart controlPart = BodyPart.HEAD;
    bool[] tutorialTriggers = new bool[1]; //trigger 1: no body UI until first thing pulled

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        ControlLegs();
    }

    private void ControlLegs()
    {
        if (tutorialTriggers[0]) //only allow leg control if the player has opened that lever
        {
            if (Input.GetKeyDown(KeyCode.H))//Input.GetMouseButtonDown(0))
            {
                if (controlPart != BodyPart.LEFTLEG) //if not controlling the left leg, try and control it
                {
                    if (controlPart == BodyPart.RIGHTLEG) //if currently controlling right ley, make it interactable
                    {
                        controlPartGO.layer = LayerMask.NameToLayer("isInteractable");
                        controlPartGO.GetComponent<Outline>().OutlineColor = new Color(1, 1, 1, 0);
                    }
                    controlPartGO = parts.controlPart(BodyPart.LEFTLEG, gameObject); //try and get the left leg
                    if (controlPartGO != null) //if you got it, 
                    {
                        controlPart = BodyPart.LEFTLEG; //record the left leg being controlled,
                        controlPartGO.layer = LayerMask.NameToLayer("Default"); //and make it ungrabbable
                        controlPartGO.GetComponent<Outline>().OutlineColor = Color.yellow;
                    }
                }
                else //if controlling left leg, snap back to body
                {
                    controlPart = BodyPart.HEAD;
                    controlPartGO.layer = LayerMask.NameToLayer("isInteractable");
                    controlPartGO.GetComponent<Outline>().OutlineColor = new Color(1, 1, 1, 0);
                    controlPartGO = null;
                }
            }
            else if (Input.GetKeyDown(KeyCode.G))//Input.GetMouseButtonDown(1))
            {
                if (controlPart != BodyPart.RIGHTLEG) //if not controlling the right leg, try and control it
                {
                    if (controlPart == BodyPart.LEFTLEG)
                    {
                        controlPartGO.layer = LayerMask.NameToLayer("isInteractable");
                        controlPartGO.GetComponent<Outline>().OutlineColor = new Color(1, 1, 1, 0);
                    }
                    controlPartGO = parts.controlPart(BodyPart.RIGHTLEG, gameObject);
                    if (controlPartGO != null)
                    {
                        controlPart = BodyPart.RIGHTLEG;
                        controlPartGO.layer = LayerMask.NameToLayer("Default");
                        controlPartGO.GetComponent<Outline>().OutlineColor = Color.yellow;
                    }
                }
                else //if controlling right leg, snap back to body
                {
                    controlPart = BodyPart.HEAD;
                    controlPartGO.layer = LayerMask.NameToLayer("isInteractable");
                    controlPartGO.GetComponent<Outline>().OutlineColor = new Color(1, 1, 1, 0);
                    controlPartGO = null;
                }
            }
        }
    }

    protected override void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (controlPartGO != null)
        {
            if (grounded) { controlPartGO.GetComponent<CharacterController>().SimpleMove(moveDir.normalized * moveSpeed * 10f); }
            else if (!grounded) { controlPartGO.GetComponent<CharacterController>().SimpleMove(moveDir.normalized * moveSpeed * 10f * airMultiplier); }
            if (grounded) { characterController.SimpleMove(Vector3.zero); } //Character controller does not like having no input for some reason
            else if (!grounded) { characterController.SimpleMove(Vector3.zero); }
        }
        else if(parts.canMove())
        {
            if (grounded) { characterController.SimpleMove(moveDir.normalized * moveSpeed * 10f); }
            else if (!grounded) { characterController.SimpleMove(moveDir.normalized * moveSpeed * 10f * airMultiplier); }
        }
        else{
            if (grounded) { characterController.SimpleMove(Vector3.zero); } //Character controller does not like having no input for some reason
            else if (!grounded) { characterController.SimpleMove(Vector3.zero); }
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
            if(tutorialTriggers[0] == false)
            {
                tutorialTriggers[0] = true;
            }
        }
        else
        {
            parts.addPart(i.partNeeded);
        }
        base.interact(i);
    }
}
