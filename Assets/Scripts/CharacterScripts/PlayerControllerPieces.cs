using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//keep track of pieces, and how to control a different piece
public class PlayerControllerPieces : PlayerController
{
    //Copy of the body
    [Header("Pieces")]
    [SerializeField] Body parts;
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
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
